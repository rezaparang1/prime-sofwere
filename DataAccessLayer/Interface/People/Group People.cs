using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity;

namespace DataAccessLayer.Interface.People
{
    public interface IGroupPeopleRepository
    {
        Task<IEnumerable<BusinessEntity.People.Group_People>> GetAll();
        Task<BusinessEntity.People.Group_People?> GetById(int id);
        Task<string> Create(int UserId , BusinessEntity.People.Group_People Group_People);
        Task<string> Update(int UserId ,BusinessEntity.People.Group_People Group_People);
        Task<string> Delete(int UserId ,int id);
    }
}
