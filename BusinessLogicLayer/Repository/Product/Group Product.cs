using FluentValidation;
using BusinessLogicLayer.Interface.Producr;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository.Product
{
    public class GroupProductService : Interface.Producr.IGroupProductService
    {
        private readonly DataAccessLayer.Interface.Product.IGroupProductRepository _GroupProductRepository;
        private readonly ILogger<GroupProductService> _logger;

        public GroupProductService(DataAccessLayer.Interface.Product.IGroupProductRepository GroupProductRepository, ILogger<GroupProductService> logger)
        {
            _GroupProductRepository = GroupProductRepository;
            _logger = logger;
        }
        //*******READ*********
        public async Task<IEnumerable<BusinessEntity.Product.Group_Product>> GetAll()
        {
            _logger.LogInformation("Request to receive all Group_Product");
            var result = await _GroupProductRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.Product.Group_Product?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Group_Product with ID: {Id}", id);
            var entity = await _GroupProductRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("Group_Product with ID {Id} not found", id);
            else
                _logger.LogInformation("Group_Product with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********
        public async Task<ServiceResult> Create(int UserId, BusinessEntity.Product.Group_Product Group_Product)
        {
            _logger.LogInformation("Request to add new Group_Product: {@Group_Product}", Group_Product);

            var validator = new ValidatData.Product.GroupProductValidator();
            var result = validator.Validate(Group_Product);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Group_Product: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var repoResult = await _GroupProductRepository.Create(UserId, Group_Product);

            var result1 = new ServiceResult(repoResult.Success, repoResult.Message);

            _logger.LogInformation("Add result: {@Result}", result1);
            return result1;
        }
        public async Task<ServiceResult> Update(int UserId, BusinessEntity.Product.Group_Product Group_Product)
        {
            _logger.LogInformation("Request to update Group_Product: {@Group_Product}", Group_Product);

            var validator = new ValidatData.Product.GroupProductValidator();
            var result = validator.Validate(Group_Product);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Group_Product: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var existing = await _GroupProductRepository.GetById(Group_Product.Id);
            if (existing == null)
            {
                _logger.LogWarning("Group_Product with ID: {Id} not found for update.", Group_Product.Id);
                throw new KeyNotFoundException(" درخواست مورد نظر یافت نشد.");
            }

            var repoResult = await _GroupProductRepository.Update(UserId, Group_Product);
            var result1 = new ServiceResult(repoResult.Success, repoResult.Message);

            _logger.LogInformation("Add result: {@Result}", result1);
            return result1;
        }
        public async Task<ServiceResult> Delete(int UserId, int id)
        {
            _logger.LogInformation("Request to delete Group_Product with ID: {Id}", id);
            var repoResult = await _GroupProductRepository.Delete(UserId,id);
            var result1 = new ServiceResult(repoResult.Success, repoResult.Message);

            _logger.LogInformation("Add result: {@Result}", result1);
            return result1;
        }
    }
}
