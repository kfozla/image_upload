using System.Net.Mime;
using api_image_upload.Data;
using api_image_upload.Models;
using Microsoft.EntityFrameworkCore;

namespace api_image_upload.Repositories;

public class ImageRepository: IImageRepository
{
    private readonly AppDbContext _context;
    public ImageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveImageAsync(ImageModel image)
    {
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ImageModel>> GetAllImagesAsync()
    {
        return await _context.Images.ToListAsync();
    }

    public async Task<ImageModel> GetImage(int id)
    {
        return await _context.Images.FindAsync(id);
    }

    public async Task<ImageModel> DeleteImageAsync(int id)
    {
        var image = await _context.Images.FindAsync(id);
        _context.Images.Remove(image);
        
        await _context.SaveChangesAsync();
        return image;
    }

    public async Task<List<ImageModel>> GetImagesByUsername(string username)
    {
        var images = _context.Images.Where(img =>img.UserName == username);
        return await images.ToListAsync();
    }
}