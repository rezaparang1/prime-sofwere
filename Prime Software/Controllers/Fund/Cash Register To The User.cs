using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Repository.Bank;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prime_Software.Controllers.Fund
{
    [Route("api/Fund/Cash_Register_To_The_User")]
    [ApiController]
    [Authorize]
    public class Cash_Register_To_The_User : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.Fund.ICashRegisterToTheUserService _CashRegisterToTheUserService;
        private readonly ILogger<Cash_Register_To_The_User> _logger;
        public Cash_Register_To_The_User(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Fund.ICashRegisterToTheUserService CashRegisterToTheUserService, ILogger<Cash_Register_To_The_User> logger)
        {
            _currentUser = currentUser;
            _CashRegisterToTheUserService = CashRegisterToTheUserService;
            _logger = logger;
        }
        //******READ******
        [HttpGet("get_regster")]
        public async Task<IActionResult> GetActiveCashRegistersForCombo()
        {
            _logger.LogInformation("Search request for GetActiveCashRegistersForCombo");
            try
            {
                var search = await _CashRegisterToTheUserService.GetActiveCashRegistersForCombo();
                _logger.LogInformation("{Count} results found.", search.Count);
                return Ok(search);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in search operation for GetActiveCashRegistersForCombo");
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all Cash_Register_To_The_User");
            var getall = await _CashRegisterToTheUserService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.Fund.Cash_Register_To_The_User>> GetById(int id)
        {
            _logger.LogInformation("Request to receive Cash_Register_To_The_User with ID: {Id}", id);
            var getbyid = await _CashRegisterToTheUserService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("Cash_Register_To_The_User with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Fund.Cash_Register_To_The_User Cash_Register_To_The_User)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new Cash_Register_To_The_User: {@Cash_Register_To_The_User}", Cash_Register_To_The_User);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {

                var result = await _CashRegisterToTheUserService.Create(userId, Cash_Register_To_The_User);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Cash_Register_To_The_User.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Cash_Register_To_The_User: {@Cash_Register_To_The_User}", Cash_Register_To_The_User);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Fund.Cash_Register_To_The_User Cash_Register_To_The_User)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@Cash_Register_To_The_User}", id, Cash_Register_To_The_User);

            if (id != Cash_Register_To_The_User.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }

            try
            {
                var result = await _CashRegisterToTheUserService.Update(userId, Cash_Register_To_The_User);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Cash_Register_To_The_User.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Cash_Register_To_The_User: {@Cash_Register_To_The_User}", Cash_Register_To_The_User);
                return BadRequest(ex.Message);
            }
        }
    }
}
