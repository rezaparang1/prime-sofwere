using DataAccessLayer.Repository.Product;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository.Product
{
    public class SectionProductService : Interface.Producr.ISectionProductService
    {
        private readonly DataAccessLayer.Interface.Product.ISectionProductRepository _SectionProductRepository;
        private readonly ILogger<SectionProductService> _logger;

        public SectionProductService(DataAccessLayer.Interface.Product.ISectionProductRepository SectionProductRepository, ILogger<SectionProductService> logger)
        {
            _SectionProductRepository = SectionProductRepository;
            _logger = logger;
        }
        //*******READ*********
        public async Task<IEnumerable<BusinessEntity.Product.Section_Product>> GetAll()
        {
            _logger.LogInformation("Request to receive all Section_Product");
            var result = await _SectionProductRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.Product.Section_Product?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Section_Product with ID: {Id}", id);
            var entity = await _SectionProductRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("Section_Product with ID {Id} not found", id);
            else
                _logger.LogInformation("Section_Product with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********
        public async Task<string> Create(int UserId, BusinessEntity.Product.Section_Product Section_Product)
        {
            _logger.LogInformation("Request to add new Section_Product: {@Section_Product}", Section_Product);

            var validator = new ValidatData.Product.SectionProductValidator();
            var result = validator.Validate(Section_Product);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Section_Product: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var message = await _SectionProductRepository.Create(UserId, Section_Product);
            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }
        public async Task<string> Update(int UserId, BusinessEntity.Product.Section_Product Section_Product)
        {
            _logger.LogInformation("Request to update Section_Product: {@Section_Product}", Section_Product);

            var validator = new ValidatData.Product.SectionProductValidator();
            var result = validator.Validate(Section_Product);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Section_Product: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var existing = await _SectionProductRepository.GetById(Section_Product.Id);
            if (existing == null)
            {
                _logger.LogWarning("Section_Product with ID: {Id} not found for update.", Section_Product.Id);
                throw new KeyNotFoundException(" درخواست مورد نظر یافت نشد.");
            }

            var message = await _SectionProductRepository.Update(UserId, Section_Product);
            _logger.LogInformation("Update result: {Message}", message);
            return message;
        }
        public async Task<string> Delete(int UserId, int id)
        {
            _logger.LogInformation("Request to delete Section_Product with ID: {Id}", id);
            var message = await _SectionProductRepository.Delete(UserId,id);
            _logger.LogInformation("Delete result: {Message}", message);
            return message;
        }
    }
}
