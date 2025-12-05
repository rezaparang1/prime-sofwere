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
    [Route("api/Product/Type_Product")]
    [ApiController]
    [Authorize]
    public class Type_Product : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.Producr.ITypeProductService _TypeProductService;
        private readonly ILogger<Type_Product> _logger;
        public Type_Product(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Producr.ITypeProductService TypeProductService, ILogger<Type_Product> logger)
        {
            _currentUser = currentUser;
            _TypeProductService = TypeProductService;
            _logger = logger;
        }
        //******READ******
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all Type_Product");
            var getall = await _TypeProductService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.Product.Type_Product>> GetById(int id)
        {
            _logger.LogInformation("Request to receive Type_Product with ID: {Id}", id);
            var getbyid = await _TypeProductService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("Type_Product with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Product.Type_Product Type_Product)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new Type_Product: {@Type_Product}", Type_Product);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _TypeProductService.Create(userId, Type_Product);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Type_Product.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating BanType_ProductkT: {@Type_Product}", Type_Product);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Product.Type_Product Type_Product)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@Type_Product}", id, Type_Product);

            if (id != Type_Product.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }
            try
            {
                var result = await _TypeProductService.Update(userId, Type_Product);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Type_Product.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Type_Product: {@Type_Product}", Type_Product);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to delete Type_Product with ID: {Id}", id);
            try
            {
                var result = await _TypeProductService.Delete(userId, id);
                if (result.Contains("شناسه"))
                {
                    _logger.LogWarning("Type_Product with ID {Id} not found for deletion", id);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully deleted Type_Product with ID {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Type_Product with ID: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
