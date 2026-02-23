using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Customer_Club;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prime_Software.Controllers.Customer_Club
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IWalletService _walletService;

        public CustomersController(ICustomerService customerService, IWalletService walletService)
        {
            _customerService = customerService;
            _walletService = walletService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
     [FromQuery] string? firstName = null,
     [FromQuery] string? lastName = null,
     [FromQuery] int? customerLevelId = null,
     [FromQuery] int? minPoints = null,
     [FromQuery] int? maxPoints = null,
     [FromQuery] string? barcode = null)
        {
            var result = await _customerService.SearchCustomersAsync(
                firstName, lastName, customerLevelId, minPoints, maxPoints, barcode);

            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _customerService.GetAllCustomersAsync();
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CustomerRegisterDto dto)
        {
            var result = await _customerService.RegisterCustomerAsync(dto);
            if (!result.IsSuccess)                          // ✅ اصلاح Success به IsSuccess
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data, message = result.Message });
        }

        [HttpGet("barcode/{barcode}")]
        public async Task<IActionResult> GetByBarcode(string barcode)
        {
            var result = await _customerService.GetCustomerByBarcodeAsync(barcode);
            if (!result.IsSuccess)                          // ✅ اصلاح Success به IsSuccess
                return NotFound(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customerService.GetCustomerByIdAsync(id);
            if (!result.IsSuccess)                          // ✅ اصلاح Success به IsSuccess
                return NotFound(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("{id}/wallet")]
        public async Task<IActionResult> GetWallet(int id)
        {
            var result = await _walletService.GetWalletByCustomerIdAsync(id);
            if (!result.IsSuccess)                          // ✅ اصلاح Success به IsSuccess
                return NotFound(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("{id}/points")]
        public async Task<IActionResult> GetPoints(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (!customer.IsSuccess)                        // ✅ اصلاح Success به IsSuccess
                return NotFound(new { success = false, message = customer.Message });

            var balance = await _walletService.GetBalanceAsync(id);
            var level = await _customerService.GetCustomerCurrentLevelAsync(id);

            return Ok(new
            {
                success = true,
                data = new
                {
                    currentPoints = customer.Data?.CurrentPoints ?? 0,
                    walletBalance = balance.Data,
                    currentLevel = level.Data?.Name,
                    levelDiscount = level.Data?.DiscountPercent
                }
            });
        }

        [HttpGet("by-people/{peopleId}")]
        public async Task<IActionResult> GetByPeopleId(int peopleId)
        {
            var result = await _customerService.GetCustomerByPeopleIdAsync(peopleId);
            if (!result.IsSuccess)                          // ✅ اصلاح Success به IsSuccess
                return NotFound(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }
    }
}
