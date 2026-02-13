using BusinessEntity.DTO.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Product
{
    public interface IPeopleRepository
    {
        Task<List<PeopleComboDto>> GetPeopleForComboAsync();
        Task<List<BusinessEntity.People.People>> Search(string? firstName = null,
            string? lastName = null, string? phone = null, string? address = null,
            int? groupPeople = null, bool? business = null, bool? user = null,
            bool? employee = null, bool? investor = null);

        Task<IEnumerable<BusinessEntity.People.People>> GetAll();
        Task<BusinessEntity.People.People?> GetById(int id);
        Task<BusinessEntity.People.People?> GetPeopleById(string? idPeople);
        Task<Result> Create(BusinessEntity.People.People person);
        Task<Result> Update(BusinessEntity.People.People person);
        Task<Result> Delete(int id);
    }
}
