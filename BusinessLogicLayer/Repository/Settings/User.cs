using BusinessEntity.Settings;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository.Settings
{
    public class UserService : Interface.Settings.IUserService
    {
        private readonly DataAccessLayer.Interface.Settings.IUserRepository _UserRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(DataAccessLayer.Interface.Settings.IUserRepository UserRepository, ILogger<UserService> logger)
        {
            _UserRepository = UserRepository;
            _logger = logger;
        }
        //*******READ*********
        public async Task<List<UserComboDto>> GetActiveUsersAsync()
        {
            _logger.LogInformation("Request Fund GetInventoryDetails ");
            var result = await _UserRepository.GetActiveUsersAsync();
            _logger.LogInformation("{Count} results found", result.Count);
            return result;
            //return _repo.GetActiveUsersAsync();
        }
        public async Task<IEnumerable<BusinessEntity.Settings.User>> GetAll()
        {
            _logger.LogInformation("Request to receive all User");
            var result = await _UserRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.Settings.User?> GetById(int id)
        {
            _logger.LogInformation("Request to receive User with ID: {Id}", id);
            var entity = await _UserRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("User with ID {Id} not found", id);
            else
                _logger.LogInformation("User with ID {Id} was successfully found", id);

            return entity;
        }
        public async Task<BusinessEntity.Settings.User?> FindByUserNameAndPassword(string? UserName = null, string? Password = null)
        {
            _logger.LogInformation("Request to receive User with UserName , Password: {UserName}{Password}", UserName, Password);
            var entity = await _UserRepository.FindByUserNameAndPassword(UserName, Password);
            if (entity == null)
                _logger.LogWarning("User with Async {UserName}{Password} not found", UserName, Password);
            else
                _logger.LogInformation("User with Async {UserName}{Password} was successfully found", UserName, Password);
            return entity;
        }
        //*****CRUD**********
        public async Task<string> Create(int UserId ,BusinessEntity.Settings.User User)
        {
            _logger.LogInformation("Request to add new User: {@User}", User);

            var validator = new ValidatData.Settings.UserValidator();
            var result = validator.Validate(User);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating User: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var message = await _UserRepository.Create(UserId ,User);
            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }
        public async Task<string> Update(int UserId ,BusinessEntity.Settings.User User)
        {
            _logger.LogInformation("Request to update User: {@User}", User);

            var validator = new ValidatData.Settings.UserValidator();
            var result = validator.Validate(User);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating User: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var existing = await _UserRepository.GetById(User.Id);
            if (existing == null)
            {
                _logger.LogWarning("User with ID: {Id} not found for update.", User.Id);
                throw new KeyNotFoundException(" درخواست مورد نظر یافت نشد.");
            }

            var message = await _UserRepository.Update(UserId ,User);
            _logger.LogInformation("Update result: {Message}", message);
            return message;
        }
    }

}
