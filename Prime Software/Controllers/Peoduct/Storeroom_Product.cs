using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Interface.Producr;
using BusinessLogicLayer.Repository.Fund;
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
    public class StoreroomProductController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IStoreroomProductService _storeroomService;

        public StoreroomProductController(
            ICurrentUserService currentUser,
            IStoreroomProductService storeroomService)
        {
            _currentUser = currentUser;
            _storeroomService = storeroomService;
        }

        // GET: api/StoreroomProduct
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _storeroomService.GetAll();
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

        // GET: api/StoreroomProduct/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _storeroomService.GetById(id);
                if (result == null)
                    return NotFound(new { message = "انبار یافت نشد." });

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

        // GET: api/StoreroomProduct/search
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? name = null,
            [FromQuery] int? sectionProductId = null,
            [FromQuery] int? peopleId = null)
        {
            try
            {
                var result = await _storeroomService.Search(name, sectionProductId, peopleId);
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

        // POST: api/StoreroomProduct
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Product.Storeroom_Product storeroom)
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

                var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException("کاربر شناسایی نشد.");
                var result = await _storeroomService.Create(storeroom, userId);

                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = storeroom.Id },
                    new
                    {
                        message = result.Message,
                        storeroomId = storeroom.Id
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

        // PUT: api/StoreroomProduct/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Product.Storeroom_Product storeroom)
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

                if (id != storeroom.Id)
                    return BadRequest(new { message = "شناسه ارسال شده با شناسه انبار مطابقت ندارد." });

                var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException("کاربر شناسایی نشد.");
                var result = await _storeroomService.Update(storeroom, userId);

                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });

                return Ok(new
                {
                    message = result.Message,
                    storeroomId = storeroom.Id
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

        // DELETE: api/StoreroomProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException("کاربر شناسایی نشد.");
                var result = await _storeroomService.Delete(id, userId);

                if (!result.IsSuccess)
                {
                    if (result.Message.Contains("یافت نشد"))
                        return NotFound(new { message = result.Message });

                    return BadRequest(new { message = result.Message });
                }

                return Ok(new
                {
                    message = result.Message,
                    storeroomId = id
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
