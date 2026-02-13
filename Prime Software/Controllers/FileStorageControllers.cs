using BusinessLogicLayer.Interface.Producr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prime_Software.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductImageController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IProductService _productService;
        private readonly ICurrentUserService _currentUser;

        public ProductImageController(
            IFileStorageService fileStorageService,
            IProductService productService,
            ICurrentUserService currentUser)
        {
            _fileStorageService = fileStorageService;
            _productService = productService;
            _currentUser = currentUser;
        }

        // POST: api/ProductImage/upload/5
        [HttpPost("upload/{productId}")]
        public async Task<IActionResult> UploadImage(int productId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new { message = "فایلی انتخاب نشده است" });

                // بررسی وجود محصول
                var product = await _productService.GetById(productId);
                if (product == null)
                    return NotFound(new { message = "محصول یافت نشد" });

                // حذف عکس قبلی اگر وجود دارد
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    await _fileStorageService.DeleteImageAsync(product.ImagePath);
                }

                // ذخیره عکس جدید
                var newImagePath = await _fileStorageService.SaveImageAsync(file, "products");

                // آپدیت مسیر عکس در محصول
                product.ImagePath = newImagePath;
                var result = await _productService.Update(product, _currentUser.UserId!.Value);

                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });

                return Ok(new
                {
                    message = "تصویر محصول با موفقیت آپلود شد",
                    imagePath = newImagePath,
                    imageUrl = _fileStorageService.GetImageUrl(newImagePath)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "خطا در آپلود تصویر محصول",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/ProductImage/5
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteImage(int productId)
        {
            try
            {
                var product = await _productService.GetById(productId);
                if (product == null)
                    return NotFound(new { message = "محصول یافت نشد" });

                if (string.IsNullOrEmpty(product.ImagePath))
                    return BadRequest(new { message = "این محصول تصویری ندارد" });

                // حذف فایل
                await _fileStorageService.DeleteImageAsync(product.ImagePath);

                // آپدیت محصول
                product.ImagePath = string.Empty;
                var result = await _productService.Update(product, _currentUser.UserId!.Value);

                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });

                return Ok(new { message = "تصویر محصول با موفقیت حذف شد" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "خطا در حذف تصویر",
                    error = ex.Message
                });
            }
        }
    }
}
