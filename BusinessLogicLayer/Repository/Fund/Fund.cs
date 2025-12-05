using BusinessEntity.Fund;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessLogicLayer.Repository.Fund
{
    public class FundService : Interface.Fund.IFundService
    {
        private readonly DataAccessLayer.Interface.Fund.IFundRepository _FundRepository;
        private readonly ILogger<FundService> _logger;

        public FundService(DataAccessLayer.Interface.Fund.IFundRepository FundRepository, ILogger<FundService> logger)
        {
            _FundRepository = FundRepository;
            _logger = logger;
        }
        //*******SEARCH*******
        public async Task<List<BusinessEntity.Fund.Fund>> Search(string? Name = null)
        {
            _logger.LogInformation("Request Fund search with Async: {Name}", Name);
            var result = await _FundRepository.Search(Name);
            _logger.LogInformation("{Count} results found", result.Count);
            return result;
        }
        //*******READ*********
        public async Task<List<InventoryItemDto>> GetInventoryDetails()
        {
            _logger.LogInformation("Request Fund GetInventoryDetails ");
            var result = await _FundRepository.GetInventoryDetailsAsync();
            _logger.LogInformation("{Count} results found", result.Count);
            return result;
        }
        public async Task<IEnumerable<BusinessEntity.Fund.Fund>> GetAll()
        {
            _logger.LogInformation("Request to receive all Fund");
            var result = await _FundRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.Fund.Fund?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Fund with ID: {Id}", id);
            var entity = await _FundRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("Fund with ID {Id} not found", id);
            else
                _logger.LogInformation("Fund with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********
        public async Task<string> Create(int UserId, BusinessEntity.Fund.Fund Fund)
        {
            _logger.LogInformation("Request to add new Fund: {@Fund}", Fund);

            var validator = new ValidatData.Fund.FundValidator();
            var result = validator.Validate(Fund);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Fund: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var message = await _FundRepository.Create(UserId, Fund);
            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }
        public async Task<string> Update(int UserId, BusinessEntity.Fund.Fund Fund)
        {
            _logger.LogInformation("Request to update Fund: {@Fund}", Fund);

            var validator = new ValidatData.Fund.FundValidator();
            var result = validator.Validate(Fund);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Fund: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var existing = await _FundRepository.GetById(Fund.Id);
            if (existing == null)
            {
                _logger.LogWarning("Fund with ID: {Id} not found for update.", Fund.Id);
                throw new KeyNotFoundException("بانک  مورد نظر یافت نشد.");
            }

            var message = await _FundRepository.Update(UserId, Fund);
            _logger.LogInformation("Update result: {Message}", message);
            return message;
        }
        public async Task<string> Delete(int UserId, int id)
        {
            _logger.LogInformation("Request to delete Fund with ID: {Id}", id);
            var message = await _FundRepository.Delete(id, UserId);
            _logger.LogInformation("Delete result: {Message}", message);
            return message;
        }
    }
}
