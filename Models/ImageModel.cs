namespace api_image_upload.Models;

public class ImageModel
{
    public int Id { get; set; }
    public string FileType { get; set; }
    public string FileName {get; set;}
    public string FilePath { get; set; }
    public string UserName { get; set; }
    public DateTime DateUploaded { get; set; }
    public string? posterPath { get; set; }
    
}