using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.People
{
    public interface IGroupPeopleService
    {
        Task<IEnumerable<BusinessEntity.People.Group_People>> GetAll();
        Task<BusinessEntity.People.Group_People?> GetById(int id);
        Task<string> Create(int UserId ,BusinessEntity.People.Group_People Group_People);
        Task<string> Update(int UserId ,BusinessEntity.People.Group_People Group_People);
        Task<string> Delete(int UserId ,int id);
    }
}
