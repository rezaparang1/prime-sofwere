using BusinessEntity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository.Bank
{
    public class DefinitionBankService : Interface.Bank.IDefinitionBankService
    {
        private readonly DataAccessLayer.Interface.Bank.IDefinitionBankRepository _BankTRepository;
        private readonly ILogger<DefinitionBankService> _logger;

        public DefinitionBankService(DataAccessLayer.Interface.Bank.IDefinitionBankRepository BankTRepository, ILogger<DefinitionBankService> logger)
        {
            _BankTRepository = BankTRepository;
            _logger = logger;
        }
        //*******READ*********
        public async Task<IEnumerable<BusinessEntity.Bank.Definition_Bank>> GetAll()
        {
            _logger.LogInformation("Request to receive all BankT");
            var result = await _BankTRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.Bank.Definition_Bank?> GetById(int id)
        {
            _logger.LogInformation("Request to receive BankT with ID: {Id}", id);
            var entity = await _BankTRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("BankT with ID {Id} not found", id);
            else
                _logger.LogInformation("BankT with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********
        public async Task<string> Create(int UserId, BusinessEntity.Bank.Definition_Bank BankT)
        {
            _logger.LogInformation("Request to add new BankT: {@BankT}", BankT);

            var validator = new ValidatData.Bank.DefinitionBankValidator();
            var result = validator.Validate(BankT);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating BankT: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var message = await _BankTRepository.Create(UserId ,BankT);
            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }
        public async Task<string> Update(int UserId, BusinessEntity.Bank.Definition_Bank BankT)
        {
            _logger.LogInformation("Request to update BankT: {@BankT}", BankT);

            var validator = new ValidatData.Bank.DefinitionBankValidator();
            var result = validator.Validate(BankT);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating BankT: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var existing = await _BankTRepository.GetById(BankT.Id);
            if (existing == null)
            {
                _logger.LogWarning("BankT with ID: {Id} not found for update.", BankT.Id);
                throw new KeyNotFoundException(" بانک مورد نظر یافت نشد.");
            }

            var message = await _BankTRepository.Update(UserId ,BankT);
            _logger.LogInformation("Update result: {Message}", message);
            return message;
        }
        public async Task<string> Delete(int UserId, int id)
        {
            _logger.LogInformation("Request to delete BankT with ID: {Id}", id);
            var message = await _BankTRepository.Delete(UserId , id);
            _logger.LogInformation("Delete result: {Message}", message);
            return message;
        }
    }
   
}
