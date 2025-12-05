using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Settings
{
    public interface IGroupUserRepository
    {
        Task<IEnumerable<BusinessEntity.Settings.Group_User>> GetAll();
        Task<BusinessEntity.Settings.Group_User?> GetById(int id);
        Task<string> Create(int UserId, BusinessEntity.Settings.Group_User Group_User);
        Task<string> Update(int UserId, BusinessEntity.Settings.Group_User Group_User);
        Task<string> Delete(int UserId, int id);
    }
}
