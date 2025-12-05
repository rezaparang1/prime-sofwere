using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Repository.Bank;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prime_Software.Controllers.People
{
    [Route("api/People/Type_People")]
    [ApiController]
    [Authorize]
    public class Type_People : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.People.ITypePeopleService _TypePeopleService;
        private readonly ILogger<Type_People> _logger;
        public Type_People(ICurrentUserService currentUser, BusinessLogicLayer.Interface.People.ITypePeopleService TypePeopleService, ILogger<Type_People> logger)
        {
            _currentUser = currentUser;
            _TypePeopleService = TypePeopleService;
            _logger = logger;
        }
        //******READ******
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all Type_People");
            var getall = await _TypePeopleService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.People.Type_People>> GetById(int id)
        {
            _logger.LogInformation("Request to receive Type_People with ID: {Id}", id);
            var getbyid = await _TypePeopleService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("Type_People with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.People.Type_People Type_People)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new Type_People: {@Type_People}", Type_People);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _TypePeopleService.Create(userId, Type_People);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Type_People.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Type_People: {@Type_People}", Type_People);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.People.Type_People Type_People)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@Type_People}", id, Type_People);

            if (id != Type_People.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }

            try
            {
                var result = await _TypePeopleService.Update(userId, Type_People);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Type_People.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Type_People: {@Type_People}", Type_People);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to delete Type_People with ID: {Id}", id);
            try
            {
                var result = await _TypePeopleService.Delete(userId,id);
                if (result.Contains("مطابقت ندارد"))
                {
                    _logger.LogWarning("Type_People with ID {Id} not found for deletion", id);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully deleted Type_People with ID {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Type_People  with ID: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
