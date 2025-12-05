using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Repository.Bank;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prime_Software.Controllers.Peoduct
{
    [Route("api/Product/Unit_Product")]
    [ApiController]
    [Authorize]
    public class Unit_Product : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.Producr.IUnitProductService _UnitProductService;
        private readonly ILogger<Unit_Product> _logger;
        public Unit_Product(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Producr.IUnitProductService UnitProductService, ILogger<Unit_Product> logger)
        {
            _currentUser = currentUser;
            _UnitProductService = UnitProductService;
            _logger = logger;
        }
        //******READ******
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all Unit_Product");
            var getall = await _UnitProductService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.Product.Unit_Product>> GetById(int id)
        {
            _logger.LogInformation("Request to receive StorerooUnit_ProductmProduct with ID: {Id}", id);
            var getbyid = await _UnitProductService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("Unit_Product with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Product.Unit_Product Unit_Product)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new Unit_Product: {@Unit_Product}", Unit_Product);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _UnitProductService.Create(userId, Unit_Product);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Unit_Product.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Unit_Product: {@Unit_Product}", Unit_Product);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Product.Unit_Product Unit_Product)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@Unit_Product}", id, Unit_Product);

            if (id != Unit_Product.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }
            try
            {
                var result = await _UnitProductService.Update(userId, Unit_Product);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Unit_Product.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Unit_Product: {@Unit_Product}", Unit_Product);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to delete Unit_Product with ID: {Id}", id);
            try
            {
                var result = await _UnitProductService.Delete(userId, id);
                if (result.Contains("شناسه"))
                {
                    _logger.LogWarning("Unit_Product with ID {Id} not found for deletion", id);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully deleted Unit_Product with ID {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Unit_Product with ID: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
