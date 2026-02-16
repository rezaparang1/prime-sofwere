using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Customer_Club;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prime_Software.Controllers.Customer_Club
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("search-transactions")]
        public async Task<IActionResult> SearchTransactions(
    [FromQuery] string? customerName = null,
    [FromQuery] DateTime? fromDate = null,
    [FromQuery] DateTime? toDate = null,
    [FromQuery] int? customerId = null)
        {
            var result = await _walletService.SearchTransactionsAsync(customerName, fromDate, toDate, customerId);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }

        [HttpPost("deposit")] 
        public async Task<IActionResult> Deposit([FromBody] DepositDto dto)
        {
            var result = await _walletService.DepositAsync(dto.CustomerId, dto.Amount, dto.Description);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, message = result.Message });
        }

        [HttpPost("withdraw")]   
        public async Task<IActionResult> Withdraw([FromBody] WithdrawDto dto)
        {
            var result = await _walletService.WithdrawAsync(dto.CustomerId, dto.Amount, dto.Description);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, message = result.Message });
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetWallet(int customerId)
        {
            var result = await _walletService.GetWalletByCustomerIdAsync(customerId);
            if (!result.IsSuccess)                          // ✅ اصلاح Success به IsSuccess
                return NotFound(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("{customerId}/balance")]
        public async Task<IActionResult> GetBalance(int customerId)
        {
            var result = await _walletService.GetBalanceAsync(customerId);
            return Ok(new { success = true, balance = result.Data });
        }

        [HttpGet("{customerId}/transactions")]
        public async Task<IActionResult> GetTransactions(int customerId, [FromQuery] int? count = null)
        {
            var result = await _walletService.GetTransactionsAsync(customerId, count);
            if (!result.IsSuccess)                          // ✅ اصلاح Success به IsSuccess
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }

       
    }
}
