using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Repository.Bank;
using BusinessLogicLayer.Repository.Product;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prime_Software.Controllers.Peoduct
{
    [Route("api/Product/PriceLevels")]
    [ApiController]
    [Authorize]
    public class PriceLevels : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.Producr.IPriceLevelsService _PriceLevelsService;
        private readonly ILogger<PriceLevels> _logger;
        public PriceLevels(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Producr.IPriceLevelsService PriceLevelsService, ILogger<PriceLevels> logger)
        {
            _currentUser = currentUser;
            _PriceLevelsService = PriceLevelsService;
            _logger = logger;
        }
        //******READ******
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all PriceLevels");
            var getall = await _PriceLevelsService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.Product.PriceLevels>> GetById(int id)
        {
            _logger.LogInformation("Request to receive PriceLevels with ID: {Id}", id);
            var getbyid = await _PriceLevelsService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("PriceLevels with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Product.PriceLevels PriceLevels)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new PriceLevels: {@PriceLevels}", PriceLevels);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _PriceLevelsService.Create(userId, PriceLevels);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = PriceLevels.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating PriceLevels: {@PriceLevels}", PriceLevels);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Product.PriceLevels PriceLevels)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@PriceLevels}", id, PriceLevels);

            if (id != PriceLevels.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }
            try
            {
                var result = await _PriceLevelsService.Update(userId, PriceLevels);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = PriceLevels.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating PriceLevels: {@PriceLevels}", PriceLevels);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to delete PriceLevels with ID: {Id}", id);
            try
            {
                var result = await _PriceLevelsService.Delete(userId,id);
                if (result.Contains("شناسه"))
                {
                    _logger.LogWarning("PriceLevels with ID {Id} not found for deletion", id);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully deleted StoreroomProduct with ID {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting PriceLevels with ID: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
