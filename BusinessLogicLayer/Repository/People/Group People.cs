using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BusinessLogicLayer.Repository.People
{
    public class GroupPeopleService : Interface.People.IGroupPeopleService
    {
        private readonly DataAccessLayer.Interface.People.IGroupPeopleRepository _GroupPeopleRepository;
        private readonly ILogger<GroupPeopleService> _logger;

        public GroupPeopleService(DataAccessLayer.Interface.People.IGroupPeopleRepository GroupPeopleRepository, ILogger<GroupPeopleService> logger)
        {
            _GroupPeopleRepository = GroupPeopleRepository;
            _logger = logger;
        }
        //*******READ*********
        public async Task<IEnumerable<BusinessEntity.People.Group_People>> GetAll()
        {
            _logger.LogInformation("Request to receive all Group_People");
            var result = await _GroupPeopleRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.People.Group_People?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Group_People with ID: {Id}", id);
            var entity = await _GroupPeopleRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("Group_People with ID {Id} not found", id);
            else
                _logger.LogInformation("Group_People with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********
        public async Task<string> Create(int UserId, BusinessEntity.People.Group_People Group_People)
        {
            _logger.LogInformation("Request to add new Group_People: {@Group_People}", Group_People);

            var validator = new ValidatData.People.GroupPeopleValidator();
            var result = validator.Validate(Group_People);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Group_People: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var message = await _GroupPeopleRepository.Create(UserId, Group_People);
            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }
        public async Task<string> Update(int UserId, BusinessEntity.People.Group_People Group_People)
        {
            _logger.LogInformation("Request to update Group_People: {@Group_People}", Group_People);

            var validator = new ValidatData.People.GroupPeopleValidator();
            var result = validator.Validate(Group_People);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Group_People: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var existing = await _GroupPeopleRepository.GetById(Group_People.Id);
            if (existing == null)
            {
                _logger.LogWarning("Group_People with ID: {Id} not found for update.", Group_People.Id);
                throw new KeyNotFoundException(" درخواست مورد نظر یافت نشد.");
            }

            var message = await _GroupPeopleRepository.Update(UserId, Group_People);
            _logger.LogInformation("Update result: {Message}", message);
            return message;
        }
        public async Task<string> Delete(int UserId, int id)
        {
            _logger.LogInformation("Request to delete Group_People with ID: {Id}", id);
            var message = await _GroupPeopleRepository.Delete(UserId,id);
            _logger.LogInformation("Delete result: {Message}", message);
            return message;
        }
    }
}
