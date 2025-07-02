using Microsoft.AspNetCore.Http;

namespace NFM.Business.Services.Contracts
{
    public interface IPhotoUploadService
    {
        Task<string> UploadPhotoAsync(IFormFile file);
    }
}