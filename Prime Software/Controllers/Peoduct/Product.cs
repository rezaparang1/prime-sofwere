using AutoMapper;
using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Interface.Producr;
using BusinessLogicLayer.Repository.Fund;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prime_Software.Controllers.Peoduct
{
    [Route("api/Product/Product")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IProductService _productService;

        public ProductController(
            ICurrentUserService currentUser,
            IProductService productService)
        {
            _currentUser = currentUser;
            _productService = productService;
        }


        [HttpGet("sales")]
        public async Task<IActionResult> GetSalesReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string? barcode = null)
        {
            if (startDate > endDate)
            {
                return BadRequest("تاریخ شروع نباید بعد از تاریخ پایان باشد.");
            }

           
            var report = await _productService.GetProductSalesReportByDateAsync(startDate, endDate, barcode);

            if (!report.Any())
                return NotFound("در این بازه زمانی فروشی ثبت نشده است.");

            return Ok(report);
        }
        [HttpGet("inventory")]
        public async Task<IActionResult> GetProductInventoryAsync([FromQuery] string? barcode = null)
        {

           
            var report = await _productService.GetProductInventoryAsync(barcode);

            if (!report.Any())
                return NotFound("موردی پیدا نشد .");

            return Ok(report);
        }

      //  GET: api/Product
       [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _productService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "خطای داخلی سرور",
                    error = ex.Message
                });
            }
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _productService.GetById(id);
                if (result == null)
                    return NotFound(new { message = "کالا یافت نشد." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "خطای داخلی سرور",
                    error = ex.Message
                });
            }
        }

        // GET: api/Product/combo
        [HttpGet("combo")]
        public async Task<IActionResult> GetProductsForCombo()
        {
            try
            {
                var result = await _productService.GetProductsForCombo();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "خطای داخلی سرور",
                    error = ex.Message
                });
            }
        }

        // GET: api/Product/search
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? name = null,
            [FromQuery] string? barcode = null,
            [FromQuery] int? typeProductId = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] string? description = null,
            [FromQuery] bool? isTax = null,
            [FromQuery] int? groupId = null,
            [FromQuery] int? storeroomId = null,
            [FromQuery] int? unitId = null,
            [FromQuery] int? sectionId = null)
        {
            try
            {
                var result = await _productService.Search(
                    name, barcode, typeProductId, isActive,
                    description, isTax, groupId, storeroomId,
                    unitId, sectionId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "خطای داخلی سرور",
                    error = ex.Message
                });
            }
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Product.Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        message = "داده‌های ارسالی معتبر نیستند.",
                        errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });

                var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException("کاربر شناسایی نشد.");
                var result = await _productService.Create(product, userId);

                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = product.Id },
                    new
                    {
                        message = result.Message,
                        productId = product.Id,
                    });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "خطای داخلی سرور",
                    error = ex.Message
                });
            }
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Product.Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        message = "داده‌های ارسالی معتبر نیستند.",
                        errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });

                if (id != product.Id)
                    return BadRequest(new { message = "شناسه ارسال شده با شناسه کالا مطابقت ندارد." });

                var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException("کاربر شناسایی نشد.");
                var result = await _productService.Update(product, userId);

                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });

                return Ok(new
                {
                    message = result.Message,
                    productId = product.Id
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "خطای داخلی سرور",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException("کاربر شناسایی نشد.");
                var result = await _productService.Delete(id, userId);

                if (!result.IsSuccess)
                {
                    if (result.Message.Contains("یافت نشد"))
                        return NotFound(new { message = result.Message });

                    return BadRequest(new { message = result.Message });
                }

                return Ok(new
                {
                    message = result.Message,
                    productId = id
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "خطای داخلی سرور",
                    error = ex.Message
                });
            }
        }
    }
}
