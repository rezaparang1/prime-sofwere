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
    [Route("api/Product/Group_Product")]
    [ApiController]
    [Authorize]
    public class Group_Product : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.Producr.IGroupProductService _GroupProductService;
        private readonly ILogger<Group_Product> _logger;
        public Group_Product(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Producr.IGroupProductService GroupProductService, ILogger<Group_Product> logger)
        {
            _currentUser = currentUser;
            _GroupProductService = GroupProductService;
            _logger = logger;
        }
        //******READ******
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all Group_Product");
            var getall = await _GroupProductService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.Product.Group_Product>> GetById(int id)
        {
            _logger.LogInformation("Request to receive Group_Product with ID: {Id}", id);
            var getbyid = await _GroupProductService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("Group_Product with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Product.Group_Product Group_Product)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new Group_Product: {@Group_Product}", Group_Product);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _GroupProductService.Create(userId, Group_Product);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Group_Product.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Group_Product: {@Group_Product}", Group_Product);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Product.Group_Product Group_Product)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@Group_Product}", id, Group_Product);

            if (id != Group_Product.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }
            try
            {
                var result = await _GroupProductService.Update(userId, Group_Product);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Group_Product.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Group_Product: {@Group_Product}", Group_Product);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to delete Group_Product with ID: {Id}", id);
            try
            {
                var result = await _GroupProductService.Delete(userId,id);
                if (result.Message.Contains("شناسه"))
                {
                    _logger.LogWarning("Group_Product with ID {Id} not found for deletion", id);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully deleted Group_Product with ID {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Group_Product with ID: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
