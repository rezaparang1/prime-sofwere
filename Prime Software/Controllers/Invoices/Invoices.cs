using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Interface.Invoices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.DTO;

namespace Prime_Software.Controllers.Invoices
{
    [Route("api/Invoices/Invoices")]
    [ApiController]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost("create-with-discounts")]
        public async Task<IActionResult> CreateWithDiscounts([FromBody] InvoiceCreateDto dto)
        {
            var result = await _invoiceService.CreateInvoiceWithAllDiscountsAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data, message = result.Message });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            var result = await _invoiceService.GetInvoiceByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }

        [HttpPost("{invoiceId}/use-wallet")]
        public async Task<IActionResult> UseWallet(int invoiceId, [FromBody] UseWalletDto dto)
        {
            var result = await _invoiceService.UseWalletForPaymentAsync(invoiceId, dto.CustomerId, dto.Amount);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, message = result.Message });
        }
    }
}
