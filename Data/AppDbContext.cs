using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using api_image_upload.Models;
namespace api_image_upload.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }
    public DbSet<ImageModel> Images { get; set; }
    
}