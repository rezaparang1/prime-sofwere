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
    [Route("api/Fund/Fund")]
    [ApiController]
    [Authorize]
    public class Fund : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.Fund.IFundService _FundService;
        private readonly ILogger<Fund> _logger;
        public Fund(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Fund.IFundService FundService, ILogger<Fund> logger)
        {
            _currentUser = currentUser;
            _FundService = FundService;
            _logger = logger;
        }
        //******SEARCH****
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? Name = null)
        {
            _logger.LogInformation("Search request for Async: {Name}", Name);
            try
            {
                var search = await _FundService.Search(Name);
                _logger.LogInformation("{Count} results found.", search.Count);
                return Ok(search);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in search operation for Async: {Name},", Name);
                return BadRequest(new { message = ex.Message });
            }
        }
        //******READ******
        [HttpGet("inventory-details")]
        public async Task<IActionResult> GetInventoryDetails()
        {
            _logger.LogInformation("Search request for GetInventoryDetails");
            try
            {
                var search = await _FundService.GetInventoryDetails();
                _logger.LogInformation("{Count} results found.", search.Count);
                return Ok(search);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetInventoryDetails operation for GetInventoryDetails");
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all Fund");
            var getall = await _FundService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.Fund.Fund>> GetById(int id)
        {
            _logger.LogInformation("Request to receive Fund with ID: {Id}", id);
            var getbyid = await _FundService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("Fund with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Fund.Fund Fund)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new Fund: {@Fund}", Fund);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _FundService.Create(userId, Fund);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Fund.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Fund: {@Fund}", Fund);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Fund.Fund Fund)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@Fund}", id, Fund);

            if (id != Fund.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }

            try
            {
                var result = await _FundService.Update(userId, Fund);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Fund.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Fund: {@Fund}", Fund);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to delete Fund with ID: {Id}", id);
            try
            {
                var result = await _FundService.Delete(id, userId);
                if (result.Contains("مطابقت ندارد"))
                {
                    _logger.LogWarning("Fund with ID {Id} not found for deletion", id);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully deleted Fund with ID {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Fund  with ID: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
