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
    [Route("api/Product/Storeroom_Product")]
    [ApiController]
    [Authorize]
    public class Storeroom_Product : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.Producr.IStoreroomProductService _StoreroomProductService;
        private readonly ILogger<Storeroom_Product> _logger;
        public Storeroom_Product(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Producr.IStoreroomProductService StoreroomProductService, ILogger<Storeroom_Product> logger)
        {
            _currentUser = currentUser;
            _StoreroomProductService = StoreroomProductService;
            _logger = logger;
        }
        //******SEARCH****
        [HttpGet("search")]
        public async Task<IActionResult> SearchAsync([FromQuery] string? Name = null , [FromQuery] string? Description = null, [FromQuery] int? SectionId = null)
        {
            _logger.LogInformation("Search request for name: {Name}{Description}{SectionId}", Name , Description , SectionId);
            try
            {
                var search = await _StoreroomProductService.Search(Name,Description,SectionId);
                _logger.LogInformation("{Count} results found.", search.Count);
                return Ok(search);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in search operation for name: {Name}{Description}{SectionId}", Name,Description,SectionId);
                return BadRequest(new { message = ex.Message });
            }
        }
        //******READ******
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all StoreroomProduct");
            var getall = await _StoreroomProductService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.Product.Storeroom_Product>> GetById(int id)
        {
            _logger.LogInformation("Request to receive StoreroomProduct with ID: {Id}", id);
            var getbyid = await _StoreroomProductService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("StoreroomProduct with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Product.Storeroom_Product StoreroomProduct)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new StoreroomProduct: {@StoreroomProduct}", StoreroomProduct);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _StoreroomProductService.Create(userId, StoreroomProduct);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = StoreroomProduct.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating StoreroomProduct: {@StoreroomProduct}", StoreroomProduct);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Product.Storeroom_Product StoreroomProduct)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@StoreroomProduct}", id, StoreroomProduct);

            if (id != StoreroomProduct.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }
            try
            {
                var result = await _StoreroomProductService.Update(userId, StoreroomProduct);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = StoreroomProduct.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating StoreroomProduct: {@StoreroomProduct}", StoreroomProduct);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to delete StoreroomProduct with ID: {Id}", id);
            try
            {
                var result = await _StoreroomProductService.Delete(userId, id);
                if (result.Contains("شناسه"))
                {
                    _logger.LogWarning("StoreroomProduct with ID {Id} not found for deletion", id);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully deleted StoreroomProduct with ID {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting StoreroomProduct with ID: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
