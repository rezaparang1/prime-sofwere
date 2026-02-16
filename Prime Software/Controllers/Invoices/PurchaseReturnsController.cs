using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Invoices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prime_Software.Controllers.Invoices
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseReturnsController : ControllerBase
    {
        private readonly IPurchaseReturnService _purchaseReturnService;

        public PurchaseReturnsController(IPurchaseReturnService purchaseReturnService)
        {
            _purchaseReturnService = purchaseReturnService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseReturnCreateDto dto)
        {
            var result = await _purchaseReturnService.CreatePurchaseReturnAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data, message = result.Message });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _purchaseReturnService.GetPurchaseReturnByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }
    }
}
