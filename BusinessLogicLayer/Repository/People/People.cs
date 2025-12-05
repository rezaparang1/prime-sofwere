using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BusinessLogicLayer.Repository.People
{
    public class PeopleService : Interface.People.IPeopleService
    {
        private readonly DataAccessLayer.Interface.People.IPeopleRepository _PeopleRepository;
        private readonly ILogger<PeopleService> _logger;

        public PeopleService(DataAccessLayer.Interface.People.IPeopleRepository PeopleRepository, ILogger<PeopleService> logger)
        {
            _PeopleRepository = PeopleRepository;
            _logger = logger;
        }
        //*******SEARCH*******
        public async Task<List<BusinessEntity.People.People>> Search(string? FirstName = null, string? LastName = null, string? PeoplId = null, string? Phone = null, string? Address = null, int? GropPeople = null, bool? Business = null, bool? User = null, bool? Employee = null, bool? Investor = null)
        {
            _logger.LogInformation("Request Group_People search with name filter: {FirstName}{LastName}{PeoplId}{Phone}{Address}{GropPeople}{Business}{User}{Employee}{Investor}", FirstName, LastName, PeoplId, Phone, GropPeople, Address, Business, User, Employee, Investor);
            var result = await _PeopleRepository.Search(FirstName,LastName,PeoplId,Phone,Address,GropPeople,Business,User,Employee,Investor);
            _logger.LogInformation("{Count} results found", result.Count);
            return result;
        }
        public async Task<BusinessEntity.People.People?> GetPeolpeById(string? IdPeople)
        {
            _logger.LogInformation("Request to receive Product with ID: {Id}", IdPeople);
            var entity = await _PeopleRepository.GetPeolpeById(IdPeople);
            if (entity == null)
                _logger.LogWarning("Product with ID {IdPeople} not found", IdPeople);
            else
                _logger.LogInformation("Product with ID {IdPeople} was successfully found", IdPeople);

            return entity;
        }
        //*******READ*********
        public async Task<List<BusinessEntity.People.PeopleComboDto>> GetPeopleForComboAsync()
        {
            _logger.LogInformation("Request GetPeopleForComboAsync");
            var result = await _PeopleRepository.GetPeopleForComboAsync();
            _logger.LogInformation("{Count} results found", result.Count);
            return result;
        }
        public async Task<IEnumerable<BusinessEntity.People.People>> GetAll()
        {
            _logger.LogInformation("Request to receive all People");
            var result = await _PeopleRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.People.People?> GetById(int id)
        {
            _logger.LogInformation("Request to receive People with ID: {Id}", id);
            var entity = await _PeopleRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("People with ID {Id} not found", id);
            else
                _logger.LogInformation("People with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********
        public async Task<string> Create(int UserId, BusinessEntity.People.People People)
        {
            _logger.LogInformation("Request to add new People: {@People}", People);

            var validator = new ValidatData.People.PeopleValidator();
            var result = validator.Validate(People);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating People: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var message = await _PeopleRepository.Create(UserId, People);
            _logger.LogInformation("Add result: {Message}", message);
            return message;
        }
        public async Task<string> Update(int UserId, BusinessEntity.People.People People)
        {
            _logger.LogInformation("Request to update People: {@People}", People);

            var validator = new ValidatData.People.PeopleValidator();
            var result = validator.Validate(People);

            if (!result.IsValid)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Error validating People: {Errors}", errors);
                throw new ValidationException("خطا در اعتبارسنجی : " + errors);
            }

            var existing = await _PeopleRepository.GetById(People.Id);
            if (existing == null)
            {
                _logger.LogWarning("People with ID: {Id} not found for update.", People.Id);
                throw new KeyNotFoundException(" درخواست مورد نظر یافت نشد.");
            }

            var message = await _PeopleRepository.Update(UserId, People);
            _logger.LogInformation("Update result: {Message}", message);
            return message;
        }
        public async Task<string> Delete(int UserId, int id)
        {
            _logger.LogInformation("Request to delete People with ID: {Id}", id);
            var message = await _PeopleRepository.Delete(id, UserId);
            _logger.LogInformation("Delete result: {Message}", message);
            return message;
        }
    }
}
