using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BusinessLogicLayer.Repository.Product
{
    public class UnitProductService : Interface.Producr.IUnitProductService
    {
        private readonly DataAccessLayer.Interface.Product.IUnitProductRepository _UnitProductRepository;
        private readonly ILogger<UnitProductService> _logger;

        public UnitProductService(DataAccessLayer.Interface.Product.IUnitProductRepository UnitProductRepository, ILogger<UnitProductService> logger)
        {
            _UnitProductRepository = UnitProductRepository;
            _logger = logger;
        }
        //*******READ*********
        public async Task<IEnumerable<BusinessEntity.Product.Unit_Product>> GetAll()
        {
            _logger.LogInformation("Request to receive all Unit_Product");
            var result = await _UnitProductRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.Product.Unit_Product?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Unit_Product with ID: {Id}", id);
            var entity = await _UnitProductRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("Unit_Product with ID {Id} not found", id);
            else
                _logger.LogInformation("Unit_Product with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********
        public async Task<string> Create(int UserId, BusinessEntity.Product.Unit_Product Unit_Product)
        {
            _logger.LogInformation("Request to add new Unit_Product: {@Unit_Product}", Unit_Product);

            var validator = new ValidatData.Product.UnitProductValidator();
            var result = validator.Validate(Unit_Product);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Unit_Product: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var message = await _UnitProductRepository.Create(UserId, Unit_Product);
            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }
        public async Task<string> Update(int UserId, BusinessEntity.Product.Unit_Product Unit_Product)
        {
            _logger.LogInformation("Request to update Unit_Product: {@Unit_Product}", Unit_Product);

            var validator = new ValidatData.Product.UnitProductValidator();
            var result = validator.Validate(Unit_Product);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Unit_Product: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var existing = await _UnitProductRepository.GetById(Unit_Product.Id);
            if (existing == null)
            {
                _logger.LogWarning("Unit_Product with ID: {Id} not found for update.", Unit_Product.Id);
                throw new KeyNotFoundException(" درخواست مورد نظر یافت نشد.");
            }

            var message = await _UnitProductRepository.Update(UserId, Unit_Product);
            _logger.LogInformation("Update result: {Message}", message);
            return message;
        }
        public async Task<string> Delete(int UserId, int id)
        {
            _logger.LogInformation("Request to delete Unit_Product with ID: {Id}", id);
            var message = await _UnitProductRepository.Delete(UserId,id);
            _logger.LogInformation("Delete result: {Message}", message);
            return message;
        }
    }
}
