using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BusinessLogicLayer.Repository.Settings
{
    public class GroupUserService : Interface.Settings.IGroupUserService
    {
        private readonly DataAccessLayer.Interface.Settings.IGroupUserRepository _GroupUserRepository;
        private readonly ILogger<GroupUserService> _logger;

        public GroupUserService(DataAccessLayer.Interface.Settings.IGroupUserRepository GroupUserRepository, ILogger<GroupUserService> logger)
        {
            _GroupUserRepository = GroupUserRepository;
            _logger = logger;
        }
        //*******READ*********
        public async Task<IEnumerable<BusinessEntity.Settings.Group_User>> GetAll()
        {
            _logger.LogInformation("Request to receive all Group_User");
            var result = await _GroupUserRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.Settings.Group_User?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Group_User with ID: {Id}", id);
            var entity = await _GroupUserRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("Group_User with ID {Id} not found", id);
            else
                _logger.LogInformation("Group_User with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********
        public async Task<string> Create(int UserId, BusinessEntity.Settings.Group_User Group_User)
        {
            _logger.LogInformation("Request to add new Group_User: {@Group_User}", Group_User);

            var validator = new ValidatData.Settings.GroupUserValidator();
            var result = validator.Validate(Group_User);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Group_User: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var message = await _GroupUserRepository.Create(UserId, Group_User);
            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }
        public async Task<string> Update(int UserId, BusinessEntity.Settings.Group_User Group_User)
        {
            _logger.LogInformation("Request to update Group_User: {@Group_User}", Group_User);

            var validator = new ValidatData.Settings.GroupUserValidator();
            var result = validator.Validate(Group_User);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Group_User: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var existing = await _GroupUserRepository.GetById(Group_User.Id);
            if (existing == null)
            {
                _logger.LogWarning("Group_User with ID: {Id} not found for update.", Group_User.Id);
                throw new KeyNotFoundException(" درخواست مورد نظر یافت نشد.");
            }

            var message = await _GroupUserRepository.Update(UserId, Group_User);
            _logger.LogInformation("Update result: {Message}", message);
            return message;
        }
        public async Task<string> Delete(int UserId, int id)
        {
            _logger.LogInformation("Request to delete Group_User with ID: {Id}", id);
            var message = await _GroupUserRepository.Delete(id, UserId);
            _logger.LogInformation("Delete result: {Message}", message);
            return message;
        }
    }
}
