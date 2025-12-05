using BusinessEntity.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.People
{
    public interface IPeopleService
    {
        Task<List<PeopleComboDto>> GetPeopleForComboAsync();
        Task<List<BusinessEntity.People.People>> Search(string? FirstName = null, string? LastName = null, string? PeoplId = null, string? Phone = null, string? Address = null, int? GropPeople = null, bool? Business = null, bool? User = null, bool? Employee = null, bool? Investor = null);
        Task<IEnumerable<BusinessEntity.People.People>> GetAll();
        Task<BusinessEntity.People.People?> GetById(int id);
        Task<BusinessEntity.People.People?> GetPeolpeById(string? IdPeople);
        Task<string> Create(int UserId ,BusinessEntity.People.People People);
        Task<string> Update(int UserId ,BusinessEntity.People.People People);
        Task<string> Delete(int UserId ,int id);
    }
}
