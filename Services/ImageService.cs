using api_image_upload.Models;
using api_image_upload.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics;

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
            
            
            var isImage = image.ContentType.StartsWith("image/");
            var isVideo = image.ContentType.StartsWith("video/");

            if (!isImage && !isVideo)
                continue;
            
            var folder = isImage? "uploads/images" : "uploads/videos";
            
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            
            var filePath = Path.Combine(folder, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            


            var imageModel = new ImageModel
            {
                FileName = newFileName,
                FilePath = filePath,
                UserName = username,
                DateUploaded = DateTime.UtcNow,
                FileType =isImage ?"image":"video"
            };
            if (isVideo)
            {
                var posterFolder = "uploads/posters";
                if (!Directory.Exists(posterFolder))
                    Directory.CreateDirectory(posterFolder);

                string safeFileName = Path.GetFileNameWithoutExtension(newFileName)
                    .Replace(" ", "_")
                    .Replace("'", "")
                    .Replace("\"", "");

                var posterFileName = safeFileName + ".jpg";
                var posterPath = Path.Combine(posterFolder, posterFileName);

                var ffmpegCommand = $"ffmpeg -i \\\"{filePath}\\\" -ss 00:00:01.000 -vframes 1 \\\"{posterPath}\\\"";
                
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "bash",
                        Arguments = $"-c \"{ffmpegCommand}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };
                process.Start();
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();
                Console.WriteLine(output);
                Console.WriteLine(error);
                await process.WaitForExitAsync();

                // Optional: DB'ye poster path eklemek istersen
                imageModel.posterPath = posterPath;
            }

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