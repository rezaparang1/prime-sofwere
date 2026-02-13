using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Customer_Club;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prime_Software.Controllers.Customer_Club
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")]
    public class PublicDiscountsController : ControllerBase
    {
        private readonly IPublicDiscountService _publicDiscountService;

        public PublicDiscountsController(IPublicDiscountService publicDiscountService)
        {
            _publicDiscountService = publicDiscountService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PublicDiscountCreateDto dto)
        {
            var result = await _publicDiscountService.CreatePublicDiscountAsync(dto);
            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data, message = result.Message });
        }

        [HttpGet("active/{storeId}")]
        public async Task<IActionResult> GetActiveDiscounts(int storeId)
        {
            var result = await _publicDiscountService.GetActiveDiscountsAsync(storeId);
            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _publicDiscountService.GetDiscountByIdAsync(id);
            if (!result.Success)
                return NotFound(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }

        [HttpPost("deactivate/{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _publicDiscountService.DeactivateDiscountAsync(id);
            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, message = result.Message });
        }

        [HttpGet("calculate")]
        public async Task<IActionResult> CalculateDiscount(
            [FromQuery] string barcode,
            [FromQuery] int storeId,
            [FromQuery] DateTime? purchaseTime = null)
        {
            var time = purchaseTime ?? DateTime.Now;
            var result = await _publicDiscountService.CalculatePublicDiscountAsync(barcode, time, storeId);
            return Ok(new
            {
                success = result.Success,
                discountAmount = result.Data?.DiscountAmount ?? 0,
                discountId = result.Data?.DiscountId
            });
        }
    }
}
