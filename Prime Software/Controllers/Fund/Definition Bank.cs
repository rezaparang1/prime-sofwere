using BusinessLogicLayer.Interface.Fund;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessEntity.Fund;

namespace Prime_Software.Controllers.Fund
{
    [Route("api/Bank/Definition_Bank")]
    [ApiController]
    [Authorize]
    public class DefinitionBankController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IDefinitionBankService _service;

        public DefinitionBankController(
            ICurrentUserService currentUser,
            IDefinitionBankService service)
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
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Definition_Bank bank)
        {
            var userId = _currentUser.UserId!.Value;
            var result = await _service.Create(bank, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = bank.Id }, result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Definition_Bank bank)
        {
            if (id != bank.Id)
                return BadRequest("شناسه ارسال شده با مقدار ذخیره شده مطابقت ندارد.");

            var userId = _currentUser.UserId!.Value;
            var result = await _service.Update(bank, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            var result = await _service.Delete(id, userId); // ترتیب اصلاح‌شده

            if (!result.IsSuccess && result.Message.Contains("یافت نشد"))
                return NotFound(result.Message);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
    }
}
