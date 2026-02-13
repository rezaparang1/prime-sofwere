using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Interface.People;
using BusinessLogicLayer.Interface.Producr;
using BusinessLogicLayer.Interface.Settings;
using BusinessLogicLayer.Repository.Fund;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prime_Software.Controllers.Peoduct
{
    [Route("api/Product/Type_Product")]
    [ApiController]
    [Authorize]
    public class Type_Product : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly ITypeProductService _service;

        public Type_Product(
            ICurrentUserService currentUser,
            ITypeProductService service)
        {
            _currentUser = currentUser;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.Product.Type_Product async)
        {
            var userId = _currentUser.UserId!.Value;
            var result = await _service.Create(async, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = async.Id }, result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Product.Type_Product async)
        {
            if (id != async.Id)
                return BadRequest("شناسه ارسال شده با مقدار ذخیره شده مطابقت ندارد.");

            var userId = _currentUser.UserId!.Value;
            var result = await _service.Update(async, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            var result = await _service.Delete(id, userId);

            if (!result.IsSuccess && result.Message.Contains("یافت نشد"))
                return NotFound(result.Message);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
    }
}
