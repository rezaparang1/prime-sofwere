using BusinessEntity.Fund;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository.Fund
{
    //public class CashRegisterToUserService : Interface.Fund.ICashRegisterToTheUserService
    //{
    //    private readonly DataAccessLayer.Interface.Fund.ICashRegisterToTheUserRepository _CashRegisterToTheUserRepository;
    //    private readonly ILogger<CashRegisterToUserService> _logger;

    //    public CashRegisterToUserService(DataAccessLayer.Interface.Fund.ICashRegisterToTheUserRepository CashRegisterToTheUserRepository, ILogger<CashRegisterToUserService> logger)
    //    {
    //        _CashRegisterToTheUserRepository = CashRegisterToTheUserRepository;
    //        _logger = logger;
    //    }
    //    //*******READ*********
    //    public async Task<List<BusinessEntity.Fund.CashRegisterComboItem>> GetActiveCashRegistersForCombo()
    //    {
    //        _logger.LogInformation("Request Fund search with GetActiveCashRegistersForCombo");
    //        var result = await _CashRegisterToTheUserRepository.GetActiveCashRegistersForCombo();
    //        _logger.LogInformation("{Count} results found", result.Count);
    //        return result;
    //    }
    //    public async Task<IEnumerable<CashRegisterDto>> GetAll()
    //    {
    //        _logger.LogInformation("Request to receive all Cash_Register_To_The_User");
    //        var result = await _CashRegisterToTheUserRepository.GetAll();
    //        _logger.LogInformation("{Count} items received", result.Count());
    //        return result;
    //    }
    //    public async Task<CashRegisterDto?> GetById(int id)
    //    {
    //        _logger.LogInformation("Request to receive Cash_Register_To_The_User with ID: {Id}", id);
    //        var entity = await _CashRegisterToTheUserRepository.GetById(id);
    //        if (entity == null)
    //            _logger.LogWarning("Cash_Register_To_The_User with ID {Id} not found", id);
    //        else
    //            _logger.LogInformation("Cash_Register_To_The_User with ID {Id} was successfully found", id);

    //        return entity;
    //    }
    //    //*****CRUD**********
    //    public async Task<string> Create(int UserId, Cash_Register_To_The_User Cash_Register_To_The_User)
    //    {
    //        _logger.LogInformation("Request to add new Cash_Register_To_The_User: {@Cash_Register_To_The_User}", Cash_Register_To_The_User);

    //        var validator = new ValidatData.Fund.CashRegisterUserValidator();
    //        var result = validator.Validate(Cash_Register_To_The_User);

    //        if (!result.IsValid)
    //        {
    //            var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
    //            _logger.LogWarning("Error validating Cash_Register_To_The_User: {Errors}", errors);
    //            throw new ValidationException("خطا در اعتبارسنجی : " + errors);
    //        }

    //        var message = await _CashRegisterToTheUserRepository.Create(UserId, Cash_Register_To_The_User);
    //        _logger.LogInformation("Add result: {Message}", message);
    //        return message;
    //    }
    //    public async Task<string> Update(int UserId, Cash_Register_To_The_User Cash_Register_To_The_User)
    //    {
    //        _logger.LogInformation("Request to update Cash_Register_To_The_User: {@Cash_Register_To_The_User}", Cash_Register_To_The_User);

    //        var validator = new ValidatData.Fund.CashRegisterUserValidator();
    //        var result = validator.Validate(Cash_Register_To_The_User);

    //        if (!result.IsValid)
    //        {
    //            var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
    //            _logger.LogWarning("Error validating Cash_Register_To_The_User: {Errors}", errors);
    //            throw new ValidationException("خطا در اعتبارسنجی : " + errors);
    //        }

    //        var existing = await _CashRegisterToTheUserRepository.GetById(Cash_Register_To_The_User.Id);
    //        if (existing == null)
    //        {
    //            _logger.LogWarning("Cash_Register_To_The_User with ID: {Id} not found for update.", Cash_Register_To_The_User.Id);
    //            throw new KeyNotFoundException("بانک  مورد نظر یافت نشد.");
    //        }

    //        var message = await _CashRegisterToTheUserRepository.Update(UserId, Cash_Register_To_The_User);
    //        _logger.LogInformation("Update result: {Message}", message);
    //        return message;
    //    }
    //}
}
