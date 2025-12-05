using BusinessEntity;
using BusinessLogicLayer;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prime_Software.Controllers.Bank
{
    [Route("api/Bank/Definition_Bank")]
    [ApiController]
    [Authorize]
    public class Definition_Bank : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.Bank.IDefinitionBankService _BankTService;
        private readonly ILogger<Definition_Bank> _logger;
        public Definition_Bank(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Bank.IDefinitionBankService BankTService, ILogger<Definition_Bank> logger)
        {
            _currentUser = currentUser;
            _BankTService = BankTService;
            _logger = logger;
        }
        //******READ******
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all BankT");
            var getall = await _BankTService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.Bank.Definition_Bank>> GetById(int id)
        {
            _logger.LogInformation("Request to receive BankT with ID: {Id}", id);
            var getbyid = await _BankTService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("BankT with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Bank.Definition_Bank BankT)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new BankT: {@BankT}", BankT);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _BankTService.Create(userId, BankT);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = BankT.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating BankT: {@BankT}", BankT);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Bank.Definition_Bank BankT)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@BankT}", id, BankT);

            if (id != BankT.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }
            try
            {
                var result = await _BankTService.Update(userId,BankT);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = BankT.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating BankT: {@BankT}", BankT);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to delete BankT with ID: {Id}", id);
            try
            {
                var result = await _BankTService.Delete(userId , id);
                if (result.Contains("شناسه"))
                {
                    _logger.LogWarning("BankT with ID {Id} not found for deletion", id);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully deleted BankT with ID {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting BankT with ID: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
