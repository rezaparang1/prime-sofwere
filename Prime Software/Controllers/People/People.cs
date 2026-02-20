using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Interface.People;
using BusinessLogicLayer.Repository.Fund;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prime_Software.Controllers.People
{
    [Route("api/People/People")]
    [ApiController]
    [Authorize]
    public class PeopleController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IPeopleService _service;

        public PeopleController(ICurrentUserService currentUser, IPeopleService service)
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
                return NotFound("شخص یافت نشد.");
            return Ok(result);
        }

        [HttpGet("by-code/{idPeople}")]
        public async Task<IActionResult> GetPeopleById(string idPeople)
        {
            var result = await _service.GetPeopleById(idPeople);
            if (result == null)
                return NotFound("شخص یافت نشد.");
            return Ok(result);
        }

        [HttpGet("getpeople")]
        public async Task<IActionResult> GetPeopleForCombo()
        {
            var result = await _service.GetPeopleForComboAsync();
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? firstName = null,
            [FromQuery] string? lastName = null,
            [FromQuery] string? phone = null,
            [FromQuery] string? address = null,
            [FromQuery] int? groupPeople = null,
            [FromQuery] bool? business = null,
            [FromQuery] bool? user = null,
            [FromQuery] bool? employee = null,
            [FromQuery] bool? investor = null)
        {
            var result = await _service.Search(firstName, lastName, phone, address,
                groupPeople, business, user, employee, investor);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.People.People person)
        {
            var userId = _currentUser.UserId!.Value;
            var result = await _service.Create(person, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = person.Id }, new
            {
                message = result.Message,
                personId = person.Id
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.People.People person)
        {
            if (id != person.Id)
                return BadRequest("شناسه ارسال شده با شناسه شخص مطابقت ندارد.");

            var userId = _currentUser.UserId!.Value;
            var result = await _service.Update(person, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            var result = await _service.Delete(id, userId);

            if (!result.IsSuccess)
            {
                if (result.Message.Contains("یافت نشد"))
                    return NotFound(result.Message);

                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
    }
}
