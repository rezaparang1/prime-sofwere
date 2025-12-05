using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BusinessLogicLayer.Repository.Fund
{
    public class WorkShiftService : Interface.Fund.IWorkShiftService
    {
        private readonly DataAccessLayer.Interface.Fund.IWorkShiftRepository _WorkShiftRepository;
        private readonly ILogger<WorkShiftService> _logger;

        public WorkShiftService(DataAccessLayer.Interface.Fund.IWorkShiftRepository WorkShiftRepository, ILogger<WorkShiftService> logger)
        {
            _WorkShiftRepository = WorkShiftRepository;
            _logger = logger;
        }
        //*******SEARCH*******
        public async Task<List<BusinessEntity.Fund.Work_Shift>> Search(int? FundId = null, int? UserId = null, DateTime? DateFirst = null, DateTime? DateEnd = null)
        {
            _logger.LogInformation("Request Bank search with Async: {FundId}{UserId}{DateFirst}{DateEnd}", FundId, UserId, DateFirst, DateEnd);
            var result = await _WorkShiftRepository.Search(UserId, FundId, DateFirst, DateEnd);
            _logger.LogInformation("{Count} results found", result.Count);
            return result;
        }
        //*******READ*********
        public async Task<List<BusinessEntity.Fund.ActiveShiftDto>> GetActiveShifts()
        {
            _logger.LogInformation("Request Bank search with GetActiveShifts");
            var result = await _WorkShiftRepository.GetActiveShifts();
            _logger.LogInformation("{Count} results found", result.Count);
            return result;
        }
        public async Task<IEnumerable<BusinessEntity.Fund.WorkShiftDto>> GetAll()
        {
            _logger.LogInformation("Request to receive all Work_Shift");
            var result = await _WorkShiftRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.Fund.WorkShiftDto?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Work_Shift with ID: {Id}", id);
            var entity = await _WorkShiftRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("Work_Shift with ID {Id} not found", id);
            else
                _logger.LogInformation("Work_Shift with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********
        public async Task<string> Create(int UserId, BusinessEntity.Fund.Work_Shift Work_Shift)
        {
            _logger.LogInformation("Request to add new Work_Shift: {@Work_Shift}", Work_Shift);

            //var validator = new ValidatData.Fund.WorkShiftValidator();
            //var result = validator.Validate(Work_Shift);

            //if (!result.IsValid)
            //{
            //    var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
            //    _logger.LogWarning("Error validating Work_Shift: {Errors}", errors);
            //    throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            //}

            var message = await _WorkShiftRepository.Create(UserId, Work_Shift);
            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }
        public async Task<string> Update(int UserId, BusinessEntity.Fund.Work_Shift Work_Shift)
        {
            _logger.LogInformation("Request to update Work_Shift: {@Work_Shift}", Work_Shift);

            //var validator = new ValidatData.Fund.WorkShiftValidator();
            //var result = validator.Validate(Work_Shift);

            //if (!result.IsValid)
            //{
            //    var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
            //    _logger.LogWarning("Error validating Work_Shift: {Errors}", errors);
            //    throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            //}

            var existing = await _WorkShiftRepository.GetById(Work_Shift.Id);
            if (existing == null)
            {
                _logger.LogWarning("Work_Shift with ID: {Id} not found for update.", Work_Shift.Id);
                throw new KeyNotFoundException("درخواست  مورد نظر یافت نشد.");
            }

            var message = await _WorkShiftRepository.Update(UserId, Work_Shift);
            _logger.LogInformation("Update result: {Message}", message);
            return message;
        }
    }
}
