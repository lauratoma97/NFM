using Microsoft.AspNetCore.Http;
using NFM.Business.Services.Contracts;

namespace NFM.Business.Services.Implementations
{
    public class PhotoUploadService : IPhotoUploadService
    {
        public async Task<string> UploadPhotoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return string.Empty;
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{System.Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = Path.Combine("images", uniqueFileName).Replace("\\", "/");

            return relativePath;
        }
    }
}