using BusinessLogicLayer.Interface.Fund;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prime_Software.Controllers.Fund
{
    [Route("api/Fund/Fund")]
    [ApiController]
    [Authorize]
    public class FundController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IFundService _service;

        public FundController(ICurrentUserService currentUser, IFundService service)
        {
            _currentUser = currentUser;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            if (result == null)
                return NotFound("صندوق یافت نشد.");
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? name)
        {
            var result = await _service.Search(name);
            return Ok(result);
        }

        [HttpGet("inventory-details")]
        public async Task<IActionResult> GetInventoryDetails()
        {
            var result = await _service.GetInventoryDetails();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Fund.Fund fund)
        {
            var userId = _currentUser.UserId!.Value;
            var result = await _service.Create(fund, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = fund.Id }, new
            {
                message = result.Message,
                fundId = fund.Id
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Fund.Fund fund)
        {
            if (id != fund.Id)
                return BadRequest("شناسه ارسال شده با شناسه صندوق مطابقت ندارد.");

            var userId = _currentUser.UserId!.Value;
            var result = await _service.Update(fund, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            var result = await _service.Delete(id, userId);

            if (!result.IsSuccess)
            {
                if (result.Message.Contains("یافت نشد"))
                    return NotFound(result.Message);

                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
    }
}
