using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Invoices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prime_Software.Controllers.Invoices
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesReturnsController : ControllerBase
    {
        private readonly ISalesReturnService _salesReturnService;

        public SalesReturnsController(ISalesReturnService salesReturnService)
        {
            _salesReturnService = salesReturnService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SalesReturnCreateDto dto)
        {
            var result = await _salesReturnService.CreateSalesReturnAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data, message = result.Message });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _salesReturnService.GetSalesReturnByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(new { success = false, message = result.Message });

            return Ok(new { success = true, data = result.Data });
        }
    }
}
