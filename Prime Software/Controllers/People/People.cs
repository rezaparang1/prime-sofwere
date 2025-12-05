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
    [Route("api/People/People")]
    [ApiController]
    [Authorize]
    public class People : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly BusinessLogicLayer.Interface.People.IPeopleService _PeopleService;
        private readonly ILogger<People> _logger;
        public People(ICurrentUserService currentUser, BusinessLogicLayer.Interface.People.IPeopleService PeopleService, ILogger<People> logger)
        {
            _currentUser = currentUser;
            _PeopleService = PeopleService;
            _logger = logger;
        }
        //******SEARCH****
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? FirstName = null , [FromQuery] string? LastName = null , [FromQuery] string? PeoplId = null , [FromQuery] string? Phone = null, [FromQuery] string? Address = null, [FromQuery] int? GropPeople = null, [FromQuery] bool? Business = null, [FromQuery] bool? User = null, [FromQuery] bool? Employee = null , [FromQuery] bool? Investor = null)
        {
            _logger.LogInformation("Search request for Async: {FirstName}{LastName}{PeoplId}{Phone}{Address}{GropPeople}{Business}{User}{Employee}{Investor}", FirstName, LastName, PeoplId, Phone, GropPeople, Address, Business, User, Employee, Investor);
            try
            {
                var search = await _PeopleService.Search(FirstName,LastName,PeoplId,Phone,Address,GropPeople,Business,User,Employee,Investor);
                _logger.LogInformation("{Count} results found.", search.Count);
                return Ok(search);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in search operation for Async: {FirstName}{LastName}{PeoplId}{Phone}{Address}{GropPeople}{Business}{User}{Employee}{Investor}", FirstName, LastName, PeoplId, Phone, GropPeople, Address, Business, User, Employee, Investor);
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("peopleid/{id}")]
        public async Task<IActionResult> GetByBarcode(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("کد نمی‌تواند خالی باشد.");

            var product = await _PeopleService.GetPeolpeById(id);

            if (product == null)
                return NotFound($"کد مورد نظر پیدا نشد .");

            return Ok(product);
        }
        //******READ******
        [HttpGet("getpeople")]
        public async Task<IActionResult> GetPeopleForComboAsync()
        {
            _logger.LogInformation("Search request for GetPeopleForComboAsync: ");
            try
            {
                var search = await _PeopleService.GetPeopleForComboAsync();
                _logger.LogInformation("{Count} results found.", search.Count);
                return Ok(search);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in search operation for GetPeopleForComboAsync");
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Request to receive all People");
            var getall = await _PeopleService.GetAll();
            _logger.LogInformation("{Count} items received", getall.Count());
            return Ok(getall);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessEntity.People.People>> GetById(int id)
        {
            _logger.LogInformation("Request to receive People with ID: {Id}", id);
            var getbyid = await _PeopleService.GetById(id);
            if (getbyid == null)
            {
                _logger.LogWarning("People with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(getbyid);
        }
        //******CRUD*****
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessEntity.People.People People)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to create a new People: {@People}", People);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _PeopleService.Create(userId, People);
                _logger.LogInformation("Successful creation: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = People.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating People: {@People}", People);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.People.People People)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request update for ID: {Id}, Data: {@People}", id, People);

            if (id != People.Id)
            {
                _logger.LogWarning("The submitted ID does not match the ID in the body.");
                return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
            }

            try
            {
                var result = await _PeopleService.Update(userId, People);
                _logger.LogInformation("Successful update: {Message}", result);
                return CreatedAtAction(nameof(GetById), new { id = People.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating People: {@People}", People);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.UserId!.Value;
            _logger.LogInformation("Request to delete People with ID: {Id}", id);
            try
            {
                var result = await _PeopleService.Delete(id, userId);
                if (result.Contains("مطابقت ندارد"))
                {
                    _logger.LogWarning("People with ID {Id} not found for deletion", id);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully deleted People with ID {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting People  with ID: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
