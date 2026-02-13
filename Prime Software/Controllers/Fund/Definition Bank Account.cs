using BusinessLogicLayer.Interface.Fund;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessEntity.Fund;

namespace Prime_Software.Controllers.Fund
{
    [Route("api/Bank/DefinitionBankAccount")]
    [ApiController]
    [Authorize]
    public class DefinitionBankAccountController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IDefinitionBankAccountService _service;

        public DefinitionBankAccountController(
            ICurrentUserService currentUser,
            IDefinitionBankAccountService service)
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
                return NotFound("حساب بانکی یافت نشد.");
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? accountNumber = null,
            [FromQuery] string? branchName = null,
            [FromQuery] string? branchAddres = null,
            [FromQuery] string? typeAccount = null,
            [FromQuery] string? cardNumber = null,
            [FromQuery] string? branchId = null,
            [FromQuery] string? bracnhPhone = null,
            [FromQuery] int? bankId = null)
        {
            var result = await _service.Search(accountNumber, branchName, branchAddres,
                typeAccount, cardNumber, branchId, bracnhPhone, bankId);
            return Ok(result);
        }

        [HttpGet("statement")]
        public async Task<IActionResult> GetBankStatement(
            [FromQuery] int? bankId = null,
            [FromQuery] DateTime? dateFrom = null,
            [FromQuery] DateTime? dateTo = null,
            [FromQuery] string? receiptNumber = null,
            [FromQuery] string? description = null)
        {
            var result = await _service.GetBankStatement(bankId, dateFrom, dateTo,
                receiptNumber, description);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Definition_Bank_Account bankAccount)
        {
            var userId = _currentUser.UserId!.Value;
            var result = await _service.Create(bankAccount, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = bankAccount.Id }, new
            {
                message = result.Message,
                accountId = bankAccount.Id
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Definition_Bank_Account bankAccount)
        {
            if (id != bankAccount.Id)
                return BadRequest("شناسه ارسال شده با شناسه حساب بانکی مطابقت ندارد.");

            var userId = _currentUser.UserId!.Value;
            var result = await _service.Update(bankAccount, userId);

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
