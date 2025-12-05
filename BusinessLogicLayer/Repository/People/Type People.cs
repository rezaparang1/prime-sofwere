using BusinessEntity.People;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository.People
{
    public class TypePeopleService : Interface.People.ITypePeopleService
    {
        private readonly DataAccessLayer.Interface.People.ITypePeopleRepository _TypePeopleRepository;
        private readonly ILogger<TypePeopleService> _logger;

        public TypePeopleService(DataAccessLayer.Interface.People.ITypePeopleRepository TypePeopleRepository, ILogger<TypePeopleService> logger)
        {
            _TypePeopleRepository = TypePeopleRepository;
            _logger = logger;
        }
        //*******READ*********
        public async Task<IEnumerable<BusinessEntity.People.Type_People>> GetAll()
        {
            _logger.LogInformation("Request to receive all Type_People");
            var result = await _TypePeopleRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.People.Type_People?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Type_People with ID: {Id}", id);
            var entity = await _TypePeopleRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("Type_People with ID {Id} not found", id);
            else
                _logger.LogInformation("Type_People with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********
        public async Task<string> Create(int UserId, BusinessEntity.People.Type_People Type_People)
        {
            _logger.LogInformation("Request to add new Type_People: {@Type_People}", Type_People);

            var validator = new ValidatData.People.TypePeopleValidator();
            var result = validator.Validate(Type_People);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Type_People: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var message = await _TypePeopleRepository.Create(UserId, Type_People);
            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }
        public async Task<string> Update(int UserId, BusinessEntity.People.Type_People Type_People)
        {
            _logger.LogInformation("Request to update Type_People: {@Type_People}", Type_People);

            var validator = new ValidatData.People.TypePeopleValidator();
            var result = validator.Validate(Type_People);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Type_People: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var existing = await _TypePeopleRepository.GetById(Type_People.Id);
            if (existing == null)
            {
                _logger.LogWarning("Type_People with ID: {Id} not found for update.", Type_People.Id);
                throw new KeyNotFoundException(" درخواست مورد نظر یافت نشد.");
            }

            var message = await _TypePeopleRepository.Update(UserId, Type_People);
            _logger.LogInformation("Update result: {Message}", message);
            return message;
        }
        public async Task<string> Delete(int UserId, int id)
        {
            _logger.LogInformation("Request to delete Type_People with ID: {Id}", id);
            var message = await _TypePeopleRepository.Delete(UserId,id);
            _logger.LogInformation("Delete result: {Message}", message);
            return message;
        }
    }
}
