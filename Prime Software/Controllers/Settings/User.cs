using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.DTO;
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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUser;

        public UsersController(IUserService userService, ICurrentUserService currentUser)
        {
            _userService = userService;   // مقداردهی سرویس کاربر
            _currentUser = currentUser;    // مقداردهی سرویس کاربر جاری
        }
        //تغییرات
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(new { success = true, data = users });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
                return NotFound(new { success = false, message = "کاربر یافت نشد." });
            return Ok(new { success = true, data = user });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
        {
            var userId = _currentUser.UserId!.Value;
            var result = await _userService.Create(dto, userId);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });
            return Ok(new { success = true, message = result.Message });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto dto)
        {
            var userId = _currentUser.UserId!.Value;
            var result = await _userService.Update(id, dto, userId);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });
            return Ok(new { success = true, message = result.Message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            var result = await _userService.Delete(id, userId);
            if (!result.IsSuccess)
                return BadRequest(new { success = false, message = result.Message });
            return Ok(new { success = true, message = result.Message });
        }
    }
}
