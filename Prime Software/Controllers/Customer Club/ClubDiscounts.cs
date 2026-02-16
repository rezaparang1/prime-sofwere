using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Customer_Club;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prime_Software.Controllers.Customer_Club
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClubDiscountsController : ControllerBase
    {
        private readonly IClubDiscountService _clubDiscountService;

        public ClubDiscountsController(IClubDiscountService clubDiscountService)
        {
            _clubDiscountService = clubDiscountService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] ClubDiscountSearchDto searchDto)
        {
            var result = await _clubDiscountService.SearchDiscountsAsync(searchDto);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClubDiscountCreateDto dto)
        {
            var result = await _clubDiscountService.CreateClubDiscountAsync(dto);
            if (!result.IsSuccess)                          // ✅ اصلاح Success به IsSuccess
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data, message = result.Message });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClubDiscountUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { success = false, message = "شناسه مسیر با شناسه بدنه مطابقت ندارد" });

            var result = await _clubDiscountService.UpdateClubDiscountAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data, message = result.Message });
        }

        [HttpGet("active/{storeId}")]
        public async Task<IActionResult> GetActiveDiscounts(int storeId)
        {
            var result = await _clubDiscountService.GetActiveDiscountsAsync(storeId);
            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _clubDiscountService.GetDiscountByIdAsync(id);
            if (!result.IsSuccess)                          // ✅ اصلاح Success به IsSuccess
                return NotFound(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("check-product/{productId}/{storeId}")]
        public async Task<IActionResult> CheckProductDiscount(int productId, int storeId)
        {
            var result = await _clubDiscountService.HasActiveDiscountForProductAsync(productId, storeId);
            return Ok(new { success = true, hasDiscount = result.Data });
        }

        [HttpGet("check-unit/{unitLevelId}/{storeId}")]
        public async Task<IActionResult> CheckUnitDiscount(int unitLevelId, int storeId)
        {
            var result = await _clubDiscountService.HasActiveDiscountForUnitAsync(unitLevelId, storeId);
            return Ok(new { success = true, hasDiscount = result.Data });
        }
    }
}
