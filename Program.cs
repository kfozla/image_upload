using api_image_upload.Data;
using Microsoft.EntityFrameworkCore;
using api_image_upload.Services;
using api_image_upload.Repositories;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http.Features;


var builder = WebApplication.CreateBuilder(args);

var connectionString = "server=localhost;port=3306;database=ImageUploadDb;user=root;password=new_password";

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); 
        });
});

// ...

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1073741824; // 1 GB örnek
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1073741824; // 1 GB
});


var app = builder.Build();

var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".heic"] = "image/heic";

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
    RequestPath = "/uploads",
    ContentTypeProvider = provider
    
});
// ❗ Add routing middleware
app.UseRouting();
app.UseCors("AllowFrontend");

// ❗ Add authorization if needed
app.UseAuthorization();

// ❗ Map attribute-based controllers (e.g., [ApiController])
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();

app.Run();