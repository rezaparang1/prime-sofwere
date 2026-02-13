using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Repository.Fund;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prime_Software.Controllers.Settings
{
    //[Route("api/Settings/Reminder")]
    //[ApiController]
    //[Authorize]
    //public class Reminder : ControllerBase
    //{
    //    private readonly ICurrentUserService _currentUser;
    //    private readonly BusinessLogicLayer.Interface.Settings.IReminderService _ReminderService;
    //    private readonly ILogger<Reminder> _logger;
    //    public Reminder(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Settings.IReminderService ReminderService, ILogger<Reminder> logger)
    //    {
    //        _currentUser = currentUser;
    //        _ReminderService = ReminderService;
    //        _logger = logger;
    //    }
    //    //******SEARCH****
    //    [HttpGet("search")]
    //    public async Task<IActionResult> Search([FromQuery] DateTime? Date = null , [FromQuery] string? Description = null)
    //    {
    //        var userId = _currentUser.UserId!.Value;
    //        _logger.LogInformation("Search request for Async: {Date}{Description}", Date , Description);
    //        try
    //        {
    //            var search = await _ReminderService.Search(userId , Date,Description);
    //            _logger.LogInformation("{Count} results found.", search.Count);
    //            return Ok(search);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error in search operation for Async: {Date}{Description},", Date,Description);
    //            return BadRequest(new { message = ex.Message });
    //        }
    //    }
    //    [HttpGet("searchbyuserid")]
    //    public async Task<IActionResult> SearchByUserId()
    //    {
    //        var userId = _currentUser.UserId!.Value;
    //        _logger.LogInformation("Search request for Async: {userId}", userId);
    //        try
    //        {
    //            var search = await _ReminderService.SearchByUserId(userId);
    //            _logger.LogInformation("{Count} results found.", search.Count);
    //            return Ok(search);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error in search operation for Async: {userId},", userId);
    //            return BadRequest(new { message = ex.Message });
    //        }
    //    }
    //    //******READ******
    //    [HttpGet]
    //    public async Task<IActionResult> GetAll()
    //    {
    //        _logger.LogInformation("Request to receive all Reminder");
    //        var getall = await _ReminderService.GetAll();
    //        _logger.LogInformation("{Count} items received", getall.Count());
    //        return Ok(getall);
    //    }
    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<BusinessEntity.Settings.Reminder>> GetById(int id)
    //    {
    //        _logger.LogInformation("Request to receive Reminder with ID: {Id}", id);
    //        var getbyid = await _ReminderService.GetById(id);
    //        if (getbyid == null)
    //        {
    //            _logger.LogWarning("Reminder with ID {Id} not found.", id);
    //            return NotFound();
    //        }
    //        return Ok(getbyid);
    //    }
    //    //******CRUD*****
    //    [HttpPost]
    //    public async Task<IActionResult> Create([FromBody] BusinessEntity.Settings.Reminder Reminder)
    //    {
    //        _logger.LogInformation("Request to create a new Reminder: {@Reminder}", Reminder);

    //        if (!ModelState.IsValid)
    //        {
    //            _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
    //            return BadRequest(ModelState);
    //        }

    //        try
    //        {           
    //            var result = await _ReminderService.Create(Reminder);
    //            _logger.LogInformation("Successful creation: {Message}", result);
    //            return CreatedAtAction(nameof(GetById), new { id = Reminder.Id }, result);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error creating Reminder: {@Reminder}", Reminder);
    //            return BadRequest(ex.Message);
    //        }
    //    }
    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> Update(int id ,[FromBody] BusinessEntity.Settings.Reminder Reminder)
    //    {
    //        var userId = _currentUser.UserId!.Value;
    //        _logger.LogInformation("Request update for ID: {Id}, Data: {@Reminder}", id, Reminder);

    //        if (id != Reminder.Id)
    //        {
    //            _logger.LogWarning("The submitted ID does not match the ID in the body.");
    //            return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
    //        }

    //        try
    //        {
    //            var result = await _ReminderService.Update(Reminder);
    //            _logger.LogInformation("Successful update: {Message}", result);
    //            return CreatedAtAction(nameof(GetById), new { id = Reminder.Id }, result);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error updating Reminder: {@Reminder}", Reminder);
    //            return BadRequest(ex.Message);
    //        }
    //    }
    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> Delete(int id)
    //    {
    //        var userId = _currentUser.UserId!.Value;
    //        _logger.LogInformation("Request to delete Reminder with ID: {Id}", id);
    //        try
    //        {
    //            var result = await _ReminderService.Delete(id);
    //            if (result.Contains("مطابقت ندارد"))
    //            {
    //                _logger.LogWarning("Reminder with ID {Id} not found for deletion", id);
    //                return NotFound(result);
    //            }

    //            _logger.LogInformation("Successfully deleted Reminder with ID {Id}", id);
    //            return Ok(result);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error deleting Reminder  with ID: {Id}", id);
    //            return BadRequest(ex.Message);
    //        }
    //    }
    //}
}
