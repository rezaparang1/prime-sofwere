using BusinessEntity;
using BusinessLogicLayer;
using BusinessLogicLayer.Interface.Fund;
using BusinessLogicLayer.Repository.Fund;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prime_Software.Controllers.Fund
{
    //[Route("api/Fund/Work_Shift")]
    //[ApiController]
    //[Authorize]
    //public class Work_Shift : ControllerBase
    //{
    //    private readonly ICurrentUserService _currentUser;
    //    private readonly BusinessLogicLayer.Interface.Fund.IWorkShiftService _WorkShiftService;
    //    private readonly ILogger<Work_Shift> _logger;
    //    public Work_Shift(ICurrentUserService currentUser, BusinessLogicLayer.Interface.Fund.IWorkShiftService WorkShiftService, ILogger<Work_Shift> logger)
    //    {
    //        _currentUser = currentUser;
    //        _WorkShiftService = WorkShiftService;
    //        _logger = logger;
    //    }
    //    //******SEARCH****
    //    [HttpGet("search")]
    //    public async Task<IActionResult> Search([FromQuery] int? FundId = null , [FromQuery] DateTime? DateFirst =null, [FromQuery] DateTime? DateEnd = null)
    //    {
    //        var userId = _currentUser.UserId!.Value;
    //        _logger.LogInformation("Search request for Async: {UserId}{FundId}{DateFirst}{DateEnd}", userId, FundId,DateFirst,DateEnd);
    //        try
    //        {
    //            var search = await _WorkShiftService.Search(userId, FundId , DateFirst,DateEnd);
    //            _logger.LogInformation("{Count} results found.", search.Count);
    //            return Ok(search);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error in search operation for Async: {UserId}{FundId}{DateFirst}{DateEnd}", userId, FundId, DateFirst, DateEnd);
    //            return BadRequest(new { message = ex.Message });
    //        }
    //    }
    //    //******READ******
    //    [HttpGet("activeshift")]
    //    public async Task<IActionResult> Search()
    //    {
    //        var userId = _currentUser.UserId!.Value;
    //        _logger.LogInformation("Search request for GetActiveShifts");
    //        try
    //        {
    //            var search = await _WorkShiftService.GetActiveShifts();
    //            _logger.LogInformation("{Count} results found.", search.Count);
    //            return Ok(search);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error in search operation for GetActiveShifts");
    //            return BadRequest(new { message = ex.Message });
    //        }
    //    }
    //    [HttpGet]
    //    public async Task<IActionResult> GetAll()
    //    {
    //        _logger.LogInformation("Request to receive all Work_Shift");
    //        var getall = await _WorkShiftService.GetAll();
    //        _logger.LogInformation("{Count} items received", getall.Count());
    //        return Ok(getall);
    //    }
    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<BusinessEntity.Fund.Work_Shift>> GetById(int id)
    //    {
    //        _logger.LogInformation("Request to receive Work_Shift with ID: {Id}", id);
    //        var getbyid = await _WorkShiftService.GetById(id);
    //        if (getbyid == null)
    //        {
    //            _logger.LogWarning("Work_Shift with ID {Id} not found.", id);
    //            return NotFound();
    //        }
    //        return Ok(getbyid);
    //    }
    //    //******CRUD*****
    //    [HttpPost]
    //    public async Task<IActionResult> Create([FromBody] BusinessEntity.Fund.Work_Shift Work_Shift)
    //    {
    //        var userId = _currentUser.UserId!.Value;
    //        _logger.LogInformation("Request to create a new Work_Shift: {@Work_Shift}", Work_Shift);

    //        if (!ModelState.IsValid)
    //        {
    //            _logger.LogWarning("The creation request was invalid: {Errors}", ModelState);
    //            return BadRequest(ModelState);
    //        }

    //        try
    //        {
    //            var result = await _WorkShiftService.Create(userId, Work_Shift);
    //            _logger.LogInformation("Successful creation: {Message}", result);
    //            return CreatedAtAction(nameof(GetById), new { id = Work_Shift.Id }, result);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error creating Work_Shift: {@Work_Shift}", Work_Shift);
    //            return BadRequest(ex.Message);
    //        }
    //    }
    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> Update(int id, [FromBody] BusinessEntity.Fund.Work_Shift Work_Shift)
    //    {
    //        var userId = _currentUser.UserId!.Value;
    //        _logger.LogInformation("Request update for ID: {Id}, Data: {@Work_Shift}", id, Work_Shift);

    //        if (id != Work_Shift.Id)
    //        {
    //            _logger.LogWarning("The submitted ID does not match the ID in the body.");
    //            return BadRequest("شناسه ثبت شده با مقدار ارسال شده مطابقت ندارد.");
    //        }

    //        try
    //        {
    //            var result = await _WorkShiftService.Update(userId, Work_Shift);
    //            _logger.LogInformation("Successful update: {Message}", result);
    //            return CreatedAtAction(nameof(GetById), new { id = Work_Shift.Id }, result);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error updating Work_Shift: {@Work_Shift}", Work_Shift);
    //            return BadRequest(ex.Message);
    //        }
    //    }
    //}
}
