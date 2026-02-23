using DataAccessLayer.Interface.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Prime_Software.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly BusinessLogicLayer.Interface.Settings.IUserService _users;
        private readonly ICurrentUserService _currentUser;

        public AuthController(
            ITokenService tokenService,
            BusinessLogicLayer.Interface.Settings.IUserService users,
            ICurrentUserService currentUser)
        {
            _tokenService = tokenService;
            _users = users;
            _currentUser = currentUser;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            // کاربر را با Include برای Group_User و AccessLevel بگیر
            var user = await _users.FindByUserNameAndPassword(dto.UserName, dto.Password);

            if (user == null || !user.IsActive)
                return Unauthorized();

            var token = _tokenService.GenerateToken(user);

            return Ok(token);
        }

        // API برای گرفتن اطلاعات کاربر جاری و سطح دسترسی
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Me()
        {
            var userId = _currentUser.UserId;
            if (userId == null) return Unauthorized();

            var user = await _users.GetUserEntityByIdAsync(userId.Value);
            if (user == null) return NotFound();

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.GroupUserId,
                AccessLevel = user.Group_User?.AccessLevel
            });
        }

    }

}
