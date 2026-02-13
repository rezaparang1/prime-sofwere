using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Prime_Software.Controllers
{
    public interface IFileStorageService
    {
        Task<string> SaveImageAsync(IFormFile file, string folderName);
        Task<bool> DeleteImageAsync(string filePath);
        string GetImageUrl(string filePath);
    }
}
