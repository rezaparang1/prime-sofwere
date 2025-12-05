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
    public class ReminderService : Interface.Settings.IReminderService
    {
        private readonly DataAccessLayer.Interface.Settings.IReminderRepository _ReminderRepository;
        private readonly ILogger<ReminderService> _logger;

        public ReminderService(DataAccessLayer.Interface.Settings.IReminderRepository ReminderRepository, ILogger<ReminderService> logger)
        {
            _ReminderRepository = ReminderRepository;
            _logger = logger;
        }
        //*******SEARCH*******
        public async Task<List<Reminder>> Search(int? UserId = null , DateTime? Date = null , string? Description = null)
        {
            _logger.LogInformation("Request Reminder search with Aysnc filter: {UserId}{Date}{Description}", UserId, Date,Description);
            var result = await _ReminderRepository.Search(UserId,Date,Description);
            _logger.LogInformation("{Count} results found", result.Count);
            return result;
        }
        public async Task<List<Reminder>> SearchByUserId(int? UserId = null)
        {
            _logger.LogInformation("Request Reminder search with Aysnc filter: {UserId}", UserId);
            var result = await _ReminderRepository.SearchByUserId(UserId);
            _logger.LogInformation("{Count} results found", result.Count);
            return result;
        }
        //*******READ*********
        public async Task<IEnumerable<Reminder>> GetAll()
        {
            _logger.LogInformation("Request to receive all Reminder");
            var result = await _ReminderRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<Reminder?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Reminder with ID: {Id}", id);
            var entity = await _ReminderRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("Reminder with ID {Id} not found", id);
            else
                _logger.LogInformation("Reminder with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********
        public async Task<string> Create(Reminder Reminder)
        {
            _logger.LogInformation("Request to add new Reminder: {@Reminder}", Reminder);

            var validator = new ValidatData.Settings.ReminderValidator();
            var result = validator.Validate(Reminder);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Reminder: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var message = await _ReminderRepository.Create(Reminder);
            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }
        public async Task<string> Update(Reminder Reminder)
        {
            _logger.LogInformation("Request to update Reminder: {@Reminder}", Reminder);

            var validator = new ValidatData.Settings.ReminderValidator();
            var result = validator.Validate(Reminder);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Reminder: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var existing = await _ReminderRepository.GetById(Reminder.Id);
            if (existing == null)
            {
                _logger.LogWarning("Reminder with ID: {Id} not found for update.", Reminder.Id);
                throw new KeyNotFoundException(" درخواست مورد نظر یافت نشد.");
            }

            var message = await _ReminderRepository.Update(Reminder);
            _logger.LogInformation("Update result: {Message}", message);
            return message;
        }
        public async Task<string> Delete(int id)
        {
            _logger.LogInformation("Request to delete Reminder with ID: {Id}", id);
            var message = await _ReminderRepository.Delete(id);
            _logger.LogInformation("Delete result: {Message}", message);
            return message;
        }
    }
}
