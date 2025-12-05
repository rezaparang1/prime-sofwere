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
    [Route("api/People/Group_People")]
    [ApiController]
    [Authorize]
    public class Group_People : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.People.IGroupPeopleService _GroupPeopleService;
        private readonly ILogger<Group_People> _logger;
        public Group_People(ICurrentUserService currentUser, BusinessLogicLayer.Interface.People.IGroupPeopleService GroupPeopleService, ILogger<Group_People> logger)
        {
            _currentUser = currentUser;
            _GroupPeopleService = GroupPeopleService;
            _logger = logger;
        }
        //******READ******
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all Group_People");
            var getall = await _GroupPeopleService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.People.Group_People>> GetById(int id)
        {
            _logger.LogInformation("Request to receive Group_People with ID: {Id}", id);
            var getbyid = await _GroupPeopleService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("Group_People with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.People.Group_People Group_People)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new Group_People: {@Group_People}", Group_People);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _GroupPeopleService.Create(userId, Group_People);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Group_People.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Group_People: {@Group_People}", Group_People);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.People.Group_People Group_People)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@Group_People}", id, Group_People);

            if (id != Group_People.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }

            try
            {
                var result = await _GroupPeopleService.Update(userId, Group_People);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Group_People.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Group_People: {@Group_People}", Group_People);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to delete Group_People with ID: {Id}", id);
            try
            {
                var result = await _GroupPeopleService.Delete(userId,id);
                if (result.Contains("مطابقت ندارد"))
                {
                    _logger.LogWarning("Group_People with ID {Id} not found for deletion", id);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully deleted Group_People with ID {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Group_People  with ID: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
