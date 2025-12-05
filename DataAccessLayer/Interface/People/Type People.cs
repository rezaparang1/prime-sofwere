using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.People
{
    public interface ITypePeopleRepository
    {
        Task<IEnumerable<BusinessEntity.People.Type_People>> GetAll();
        Task<BusinessEntity.People.Type_People?> GetById(int id);
        Task<string> Create(int UserId, BusinessEntity.People.Type_People Type_People);
        Task<string> Update(int UserId, BusinessEntity.People.Type_People Type_People);
        Task<string> Delete(int UserId, int id);
    }
}
