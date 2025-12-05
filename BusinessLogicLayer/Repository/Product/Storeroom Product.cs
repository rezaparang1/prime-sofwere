using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BusinessLogicLayer.Repository.Product
{
    public class StoreroomProductService : Interface.Producr.IStoreroomProductService
    {
        private readonly DataAccessLayer.Interface.Product.IStoreroomProductRepository _StoreroomProductRepository;
        private readonly ILogger<StoreroomProductService> _logger;

        public StoreroomProductService(DataAccessLayer.Interface.Product.IStoreroomProductRepository StoreroomProductRepository, ILogger<StoreroomProductService> logger)
        {
            _StoreroomProductRepository = StoreroomProductRepository;
            _logger = logger;
        }
        //*******SEARCH*******
        public async Task<List<BusinessEntity.Product.StoreroomProductDto>> Search(string? Name = null, string? Description = null, int? IdSection = null)
        {
            _logger.LogInformation("Request BankT search with name filter: {Name}{Description}{IdSection}", Name,Description, IdSection);
            var result = await _StoreroomProductRepository.Search(Name,Description,IdSection);
            _logger.LogInformation("{Count} results found", result.Count);
            return result;
        }
        //*******READ*********
        public async Task<IEnumerable<BusinessEntity.Product.StoreroomProductDto>> GetAll()
        {
            _logger.LogInformation("Request to receive all Storeroom_Product");
            var result = await _StoreroomProductRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.Product.StoreroomProductDto?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Storeroom_Product with ID: {Id}", id);
            var entity = await _StoreroomProductRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("Storeroom_Product with ID {Id} not found", id);
            else
                _logger.LogInformation("Storeroom_Product with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********
        public async Task<string> Create(int UserId, BusinessEntity.Product.Storeroom_Product Storeroom_Product)
        {
            _logger.LogInformation("Request to add new Storeroom_Product: {@Storeroom_Product}", Storeroom_Product);

            var validator = new ValidatData.Product.StoreroomProductValidator();
            var result = validator.Validate(Storeroom_Product);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Storeroom_Product: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var message = await _StoreroomProductRepository.Create(UserId, Storeroom_Product);
            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }
        public async Task<string> Update(int UserId, BusinessEntity.Product.Storeroom_Product Storeroom_Product)
        {
            _logger.LogInformation("Request to update Storeroom_Product: {@Storeroom_Product}", Storeroom_Product);

            var validator = new ValidatData.Product.StoreroomProductValidator();
            var result = validator.Validate(Storeroom_Product);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Storeroom_Product: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var existing = await _StoreroomProductRepository.GetById(Storeroom_Product.Id);
            if (existing == null)
            {
                _logger.LogWarning("Storeroom_Product with ID: {Id} not found for update.", Storeroom_Product.Id);
                throw new KeyNotFoundException(" بانک مورد نظر یافت نشد.");
            }

            var message = await _StoreroomProductRepository.Update(UserId, Storeroom_Product);
            _logger.LogInformation("Update result: {Message}", message);
            return message;
        }
        public async Task<string> Delete(int UserId, int id)
        {
            _logger.LogInformation("Request to delete Storeroom_Product with ID: {Id}", id);
            var message = await _StoreroomProductRepository.Delete(id, UserId);
            _logger.LogInformation("Delete result: {Message}", message);
            return message;
        }
    }
}
