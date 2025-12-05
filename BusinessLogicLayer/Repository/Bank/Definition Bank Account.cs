using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BusinessEntity;

namespace BusinessLogicLayer.Repository.Bank
{
    public class DefinitionBankAccountService : Interface.Bank.IDefinitionBankAccountService
    {
        private readonly DataAccessLayer.Interface.Bank.IDefinitionBankAccountRepository _BankRepository;
        private readonly ILogger<DefinitionBankAccountService> _logger;

        public DefinitionBankAccountService(DataAccessLayer.Interface.Bank.IDefinitionBankAccountRepository BankRepository, ILogger<DefinitionBankAccountService> logger)
        {
            _BankRepository = BankRepository;
            _logger = logger;
        }
        //*******SEARCH*******
        public async Task<List<BusinessEntity.Bank.Definition_Bank_Account>> Search(string? AccountNumber = null, string? BranchName = null, string? BranchAddres = null, string? TypeAccount = null, string? CardNumber = null, string? BranchId = null, string? BracnhPhone = null, int? BankId = null)
        {
            _logger.LogInformation("Request Bank search with Async: {AccountNumber},{TypeAccount},{CardNumber}", AccountNumber, TypeAccount,CardNumber);
            var result = await _BankRepository.Search(AccountNumber, TypeAccount, CardNumber);
            _logger.LogInformation("{Count} results found", result.Count);
            return result;
        }
        public async Task<IEnumerable<BusinessEntity.Bank.BankDetailedStatementDto>> GetBankStatement(int? bankId = null, DateTime? dateFrom = null, DateTime? dateTo = null, string? receiptNumber = null, string? description = null)
        {
            _logger.LogInformation("Request to receive all Bank");
            var result = await _BankRepository.GetBankStatement();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        //*******READ*********
        public async Task<IEnumerable<BusinessEntity.Bank.Definition_Bank_Account>> GetAll()
        {
            _logger.LogInformation("Request to receive all Bank");
            var result = await _BankRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.Bank.Definition_Bank_Account?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Bank with ID: {Id}", id);
            var entity = await _BankRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("Bank with ID {Id} not found", id);
            else
                _logger.LogInformation("Bank with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********
        public async Task<string> Create(int UserId, BusinessEntity.Bank.Definition_Bank_Account Bank)
        {
            _logger.LogInformation("Request to add new Bank: {@Bank}", Bank);

            var validator = new ValidatData.Bank.DefinitionBankAccountValidator();
            var result = validator.Validate(Bank);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Bank: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var message = await _BankRepository.Create(UserId ,Bank);
            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }
        public async Task<string> Update(int UserId, BusinessEntity.Bank.Definition_Bank_Account Bank)
        {
            _logger.LogInformation("Request to update Bank: {@Bank}", Bank);

            var validator = new ValidatData.Bank.DefinitionBankAccountValidator();
            var result = validator.Validate(Bank);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating Bank: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var existing = await _BankRepository.GetById(Bank.Id);
            if (existing == null)
            {
                _logger.LogWarning("Bank with ID: {Id} not found for update.", Bank.Id);
                throw new KeyNotFoundException("بانک  مورد نظر یافت نشد.");
            }

            var message = await _BankRepository.Update(UserId,Bank);
            _logger.LogInformation("Update result: {Message}", message);
            return message;
        }    
        public async Task<string> Delete(int UserId,int id)
        {
            _logger.LogInformation("Request to delete Bank with ID: {Id}", id);
            var message = await _BankRepository.Delete(id , UserId);
            _logger.LogInformation("Delete result: {Message}", message);
            return message;
        }
    }
}
