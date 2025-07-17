using System.Net.Mime;
using api_image_upload.Data;
using api_image_upload.Models;
using Microsoft.AspNetCore.Mvc;
using api_image_upload.Services;
using System.IO;


namespace api_image_upload.Controllers;
[ApiController]
[Route("api/image")]
public class ImageController: ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IImageService _service;

    public ImageController(AppDbContext context, IImageService  service)
    {
        _context = context;
        _service = service;
    }
    [RequestSizeLimit(104857600)]
    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] string username,[FromForm] List<IFormFile> images)
    {
        if (string.IsNullOrEmpty(username) || images.Count == 0 || images.Count == 0)
            return BadRequest();
       
        await _service.UploadImagesAsync(username, images);
        return Ok();
    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAllImages()
    {
        
        var images = await _service.GetAllImages();
        if (images == null)
            return NotFound();
        
        return Ok(images);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetImage(int id)
    {
        var image = await _service.GetImage(id);
        if (image == null)
            return NotFound();
        return Ok(image);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var usernameFromCookie = Request.Cookies["username"];
        var image = await _service.GetImage(id);
        if (image == null)
            throw new Exception("Image not found");
        if (usernameFromCookie != image.UserName)
            return Unauthorized();
        
        if (System.IO.File.Exists(image.FilePath))
        {
            System.IO.File.Delete(image.FilePath);
        }
        else
        {
            throw new Exception("Image not found");
        }
        await _service.DeleteImage(id);
        return Ok();
    }

    [HttpGet("user/{username}")]
    public async Task<IActionResult> UploadImage(string username)
    {
        if (string.IsNullOrEmpty(username))
            return BadRequest();
        var images = await _service.GetImagesByUsername(username);
        if (images == null)
            return NotFound();
        return Ok(images);
    }
}