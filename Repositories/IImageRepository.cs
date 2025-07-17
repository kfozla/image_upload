using System.Net.Mime;
using api_image_upload.Models;
namespace api_image_upload.Repositories;

public interface IImageRepository
{
    Task SaveImageAsync(ImageModel image);
    Task<List<ImageModel>> GetAllImagesAsync();
    Task<ImageModel> GetImage(int id);
    Task<ImageModel> DeleteImageAsync(int id);
    Task<List<ImageModel>> GetImagesByUsername(string username);
}