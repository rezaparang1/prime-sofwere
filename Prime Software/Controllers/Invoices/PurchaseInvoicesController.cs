using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Invoices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prime_Software.Controllers.Invoices
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseInvoicesController : ControllerBase
    {
        private readonly IPurchaseInvoiceService _purchaseInvoiceService;

        public PurchaseInvoicesController(IPurchaseInvoiceService purchaseInvoiceService)
        {
            _purchaseInvoiceService = purchaseInvoiceService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseInvoiceCreateDto dto)
        {
            var result = await _purchaseInvoiceService.CreatePurchaseInvoiceAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data, message = result.Message });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _purchaseInvoiceService.GetPurchaseInvoiceByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }
    }
}
