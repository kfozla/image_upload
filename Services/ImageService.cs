using api_image_upload.Models;
using api_image_upload.Repositories;

namespace api_image_upload.Services;

public class ImageService  : IImageService
{
    private readonly IImageRepository _repository;

    public ImageService(IImageRepository repository)
    {
        _repository = repository;
    }

    public async Task UploadImagesAsync(string username, List<IFormFile> images)
    {
        if (!Directory.Exists("uploads"))
            Directory.CreateDirectory("uploads");

        foreach (var image in images)
        {
            var newFileName = username + "-" + image.FileName;
            var filePath = Path.Combine("uploads", newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var imageModel = new ImageModel
            {
                FileName = newFileName,
                FilePath = filePath,
                UserName = username,
                DateUploaded = DateTime.UtcNow
            };

            await _repository.SaveImageAsync(imageModel);
        }
    }

    public Task<List<ImageModel>> GetAllImages()
    {
        return  _repository.GetAllImagesAsync();
    }

    public Task<ImageModel> GetImage(int id)
    {
        return _repository.GetImage(id);
    }

    public Task<ImageModel> DeleteImage(int id)
    {
        return _repository.DeleteImageAsync(id);
    }

    public Task<List<ImageModel>> GetImagesByUsername(string username)
    {
        return _repository.GetImagesByUsername(username);
    }
}