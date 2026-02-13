using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Interface.Settings;
using BusinessLogicLayer.Repository.Fund;
using BusinessLogicLayer.Repository.Settings;
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
    public class UserController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IUserService _userService;

        public UserController(
            ICurrentUserService currentUser,
            IUserService userService)
        {
            _currentUser = currentUser;
            _userService = userService;
        }
        [HttpGet("my-profile")]
        public async Task<ActionResult<BusinessEntity.Settings.User>> GetByUserId()
        {
            var userId = _currentUser.UserId!.Value;        
            var getbyid = await _userService.GetById(userId);
            if (getbyid == null)
            {        
                return NotFound();
            }
            return Ok(getbyid);
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _userService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "خطای داخلی سرور",
                    error = ex.Message
                });
            }
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _userService.GetById(id);
                if (result == null)
                    return NotFound(new { message = "کاربر یافت نشد." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "خطای داخلی سرور",
                    error = ex.Message
                });
            }
        }

        // GET: api/User/active-users
        [HttpGet("user_active")]
        public async Task<IActionResult> GetActiveUsers()
        {
            try
            {
                var result = await _userService.GetActiveUsersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "خطای داخلی سرور",
                    error = ex.Message
                });
            }
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Settings.User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        message = "داده‌های ارسالی معتبر نیستند.",
                        errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });

                var currentUserId = _currentUser.UserId ?? throw new UnauthorizedAccessException("کاربر شناسایی نشد.");
                var result = await _userService.Create(user, currentUserId);

                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = user.Id },
                    new
                    {
                        message = result.Message,
                        userId = user.Id
                    });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "خطای داخلی سرور",
                    error = ex.Message
                });
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Settings.User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        message = "داده‌های ارسالی معتبر نیستند.",
                        errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });

                if (id != user.Id)
                    return BadRequest(new { message = "شناسه ارسال شده با شناسه کاربر مطابقت ندارد." });

                var currentUserId = _currentUser.UserId ?? throw new UnauthorizedAccessException("کاربر شناسایی نشد.");
                var result = await _userService.Update(user, currentUserId);

                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });

                return Ok(new
                {
                    message = result.Message,
                    userId = user.Id
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "خطای داخلی سرور",
                    error = ex.Message
                });
            }
        }
    }
   
    


    }
