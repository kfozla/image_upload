using System.Net.Mime;
using api_image_upload.Models;

namespace api_image_upload.Services;

public interface IImageService
{
    Task UploadImagesAsync(string username, List<IFormFile> images);
    Task<List<ImageModel>> GetAllImages();
    Task<ImageModel> GetImage(int id);
    Task<ImageModel> DeleteImage(int id);
    Task<List<ImageModel>> GetImagesByUsername(string username);
}