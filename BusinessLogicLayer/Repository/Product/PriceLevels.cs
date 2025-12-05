using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BusinessLogicLayer.Repository.Product
{
    public class PriceLevelsService : Interface.Producr.IPriceLevelsService
    {
        private readonly DataAccessLayer.Interface.Product.IPriceLevelsRepository _PriceLevelsRepository;
        private readonly ILogger<PriceLevelsService> _logger;

        public PriceLevelsService(DataAccessLayer.Interface.Product.IPriceLevelsRepository PriceLevelsRepository, ILogger<PriceLevelsService> logger)
        {
            _PriceLevelsRepository = PriceLevelsRepository;
            _logger = logger;
        }
        //*******READ*********
        public async Task<IEnumerable<BusinessEntity.Product.PriceLevels>> GetAll()
        {
            _logger.LogInformation("Request to receive all PriceLevels");
            var result = await _PriceLevelsRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.Product.PriceLevels?> GetById(int id)
        {
            _logger.LogInformation("Request to receive PriceLevels with ID: {Id}", id);
            var entity = await _PriceLevelsRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("PriceLevels with ID {Id} not found", id);
            else
                _logger.LogInformation("PriceLevels with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********
        public async Task<string> Create(int UserId, BusinessEntity.Product.PriceLevels PriceLevels)
        {
            _logger.LogInformation("Request to add new PriceLevels: {@PriceLevels}", PriceLevels);

            var validator = new ValidatData.Product.PriceLevelsValidator();
            var result = validator.Validate(PriceLevels);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating PriceLevels: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var message = await _PriceLevelsRepository.Create(UserId, PriceLevels);
            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }
        public async Task<string> Update(int UserId, BusinessEntity.Product.PriceLevels PriceLevels)
        {
            _logger.LogInformation("Request to update PriceLevels: {@PriceLevels}", PriceLevels);

            var validator = new ValidatData.Product.PriceLevelsValidator();
            var result = validator.Validate(PriceLevels);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating PriceLevels: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var existing = await _PriceLevelsRepository.GetById(PriceLevels.Id);
            if (existing == null)
            {
                _logger.LogWarning("PriceLevels with ID: {Id} not found for update.", PriceLevels.Id);
                throw new KeyNotFoundException(" درخواست مورد نظر یافت نشد.");
            }

            var message = await _PriceLevelsRepository.Update(UserId, PriceLevels);
            _logger.LogInformation("Update result: {Message}", message);
            return message;
        }
        public async Task<string> Delete(int UserId, int id)
        {
            _logger.LogInformation("Request to delete PriceLevels with ID: {Id}", id);
            var message = await _PriceLevelsRepository.Delete(UserId,id);
            _logger.LogInformation("Delete result: {Message}", message);
            return message;
        }
    }
}
