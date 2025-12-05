using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Repository.Bank;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prime_Software.Controllers.Bank
{
    [Route("api/Bank/DefinitionBankAccount")]
    [ApiController]
    [Authorize]
    public class Definition_Bank_Account : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.Bank.IDefinitionBankAccountService _BankService;
        private readonly ILogger<Definition_Bank_Account> _logger;
        public Definition_Bank_Account(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Bank.IDefinitionBankAccountService BankService, ILogger<Definition_Bank_Account> logger)
        {
            _currentUser = currentUser;
            _BankService = BankService;
            _logger = logger;
        }
        //******SEARCH****
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? AccountNumber = null, [FromQuery] string? TypeAccount = null, [FromQuery] string? CardNumber = null)
        {
            _logger.LogInformation("Search request for Async: {AccountNumber},{TypeAccount},{CardNumber}", AccountNumber, TypeAccount, CardNumber);
            try
            {
                var search = await _BankService.Search(AccountNumber, TypeAccount, CardNumber);
                _logger.LogInformation("{Count} results found.", search.Count);
                return Ok(search);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in search operation for Async: {AccountNumber},{TypeAccount},{CardNumber},", AccountNumber, TypeAccount, CardNumber);
                return BadRequest(new { message = ex.Message });
            }
        }
        //******READ******
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all Banks");
            var getall = await _BankService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.Bank.Definition_Bank_Account>> GetById(int id)
        {
            _logger.LogInformation("Request to receive Bank with ID: {Id}", id);
            var getbyid = await _BankService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("Bank with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Bank.Definition_Bank_Account Bank)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new Bank: {@Bank}", Bank);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _BankService.Create(userId, Bank);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Bank.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Bank: {@Bank}", Bank);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Bank.Definition_Bank_Account Bank)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@Bank}", id, Bank);

            if (id != Bank.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }

            try
            {
                var result = await _BankService.Update(userId, Bank);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Bank.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Bank: {@Bank}", Bank);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to delete Bank with ID: {Id}", id);
            try
            {
                var result = await _BankService.Delete(userId , id);
                if (result.Contains("مطابقت ندارد"))
                {
                    _logger.LogWarning("Bank with ID {Id} not found for deletion", id);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully deleted Bank with ID {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Bank  with ID: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
