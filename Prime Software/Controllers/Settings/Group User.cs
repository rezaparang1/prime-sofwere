using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Repository.Bank;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Prime_Software.Controllers.Settings
{
    [Route("api/Settings/Group_User")]
    [ApiController]
    [Authorize]
    public class Group_User : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.Settings.IGroupUserService _GroupUserService;
        private readonly ILogger<Group_User> _logger;
        public Group_User(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Settings.IGroupUserService GroupUserService, ILogger<Group_User> logger)
        {
            _currentUser = currentUser;
            _GroupUserService = GroupUserService;
            _logger = logger;
        }
        //******READ******
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all Group_User");
            var getall = await _GroupUserService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.Settings.Group_User>> GetById(int id)
        {
            _logger.LogInformation("Request to receive Group_User with ID: {Id}", id);
            var getbyid = await _GroupUserService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("Group_User with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Settings.Group_User Group_User)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new Group_User: {@Group_User}", Group_User);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _GroupUserService.Create(userId, Group_User);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Group_User.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Group_User: {@Group_User}", Group_User);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Settings.Group_User Group_User)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@Group_User}", id, Group_User);

            if (id != Group_User.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }

            try
            {
                var result = await _GroupUserService.Update(userId, Group_User);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = Group_User.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Group_User: {@Group_User}", Group_User);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to delete Group_User with ID: {Id}", id);
            try
            {
                var result = await _GroupUserService.Delete(id, userId);
                if (result.Contains("مطابقت ندارد"))
                {
                    _logger.LogWarning("Group_User with ID {Id} not found for deletion", id);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully deleted Group_User with ID {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Group_User  with ID: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
