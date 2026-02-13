using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Prime_Software.Controllers
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileStorageService> _logger;

        public FileStorageService(
            IWebHostEnvironment environment,
            ILogger<FileStorageService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<string> SaveImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("فایل نامعتبر است");

            // بررسی نوع فایل
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("فرمت فایل مجاز نیست");

            // بررسی حجم فایل (5MB)
            if (file.Length > 5 * 1024 * 1024)
                throw new ArgumentException("حجم فایل نباید بیشتر از 5MB باشد");

            // ایجاد پوشه
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads", folderName);
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            // ایجاد نام یکتا
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsPath, fileName);

            // ذخیره فایل
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // برگرداندن مسیر نسبی
            var relativePath = $"/uploads/{folderName}/{fileName}";
            _logger.LogInformation($"تصویر ذخیره شد: {relativePath}");

            return relativePath;
        }

        public async Task<bool> DeleteImageAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            try
            {
                var physicalPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));

                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                    _logger.LogInformation($"تصویر حذف شد: {filePath}");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در حذف تصویر: {filePath}");
                return false;
            }
        }

        public string GetImageUrl(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return string.Empty;

            if (filePath.StartsWith("http"))
                return filePath;

            // در محیط توسعه
            var baseUrl = _environment.IsDevelopment()
                ? "https://localhost:44351"
                : "https://yourdomain.com";

            return $"{baseUrl}{filePath}";
        }
    }
}
