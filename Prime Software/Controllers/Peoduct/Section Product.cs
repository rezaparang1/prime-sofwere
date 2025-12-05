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
    [Route("api/Product/Section_Product")]
    [ApiController]
    [Authorize]
    public class Section_Product : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.Producr.ISectionProductService _SectionProductService;
        private readonly ILogger<Section_Product> _logger;
        public Section_Product(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Producr.ISectionProductService SectionProductService, ILogger<Section_Product> logger)
        {
            _currentUser = currentUser;
            _SectionProductService = SectionProductService;
            _logger = logger;
        }
        //******READ******
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all Section_Product");
            var getall = await _SectionProductService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.Product.Section_Product>> GetById(int id)
        {
            _logger.LogInformation("Request to receive Section_Product with ID: {Id}", id);
            var getbyid = await _SectionProductService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("Section_Product with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Product.Section_Product Section_Product)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new Section_Product: {@Section_Product}", Section_Product);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _SectionProductService.Create(userId, Section_Product);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Section_Product.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Section_Product: {@Section_Product}", Section_Product);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Product.Section_Product Section_Product)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@Section_Product}", id, Section_Product);

            if (id != Section_Product.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }
            try
            {
                var result = await _SectionProductService.Update(userId, Section_Product);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Section_Product.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Section_Product: {@Section_Product}", Section_Product);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to delete Section_Product with ID: {Id}", id);
            try
            {
                var result = await _SectionProductService.Delete(userId,id);
                if (result.Contains("شناسه"))
                {
                    _logger.LogWarning("Section_Product with ID {Id} not found for deletion", id);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully deleted Section_Product with ID {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Section_Product with ID: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
