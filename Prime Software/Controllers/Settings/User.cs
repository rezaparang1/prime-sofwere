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
    [Route("api/Settings/User")]
    [ApiController]
    [Authorize]
    public class User : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.Settings.IUserService _UserService;
        private readonly ILogger<User> _logger;
        public User(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Settings.IUserService UserService, ILogger<User> logger)
        {
            _currentUser = currentUser;
            _UserService = UserService;
            _logger = logger;
        }
        //******READ******
        [HttpGet("user_active")]
        public async Task<IActionResult> GetActiveUsersAsync()
        {
            _logger.LogInformation("Search request for GetActiveUsersAsync");
            try
            {
                var search = await _UserService.GetActiveUsersAsync();
                _logger.LogInformation("{Count} results users.", search.Count);
                return Ok(search);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetActiveUsersAsync operation for GetActiveUsersAsync");
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all User");
            var getall = await _UserService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.Settings.User>> GetById(int id)
        {
            _logger.LogInformation("Request to receive User with ID: {Id}", id);
            var getbyid = await _UserService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("User with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        [HttpGet("my-profile")]
        public async Task<ActionResult<BusinessEntity.Settings.User>> GetByUserId()
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to receive User with ID: {userId}", userId);
            var getbyid = await _UserService.GetById(userId);
            if (getbyid == null)
            {
                _logger.LogWarning("User with ID {userId} not found.", userId);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Settings.User User)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new User: {@User}", User);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _UserService.Create(userId, User);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = User.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating User: {@User}", User);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Settings.User User)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@User}", id, User);

            if (id != User.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }

            try
            {
                var result = await _UserService.Update(userId, User);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = User.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating User: {@User}", User);
                return BadRequest(ex.Message);
            }
        }
    }
}
