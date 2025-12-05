using BusinessEntity.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Settings
{
    public interface IUserRepository
    {
        Task<List<UserComboDto>> GetActiveUsersAsync();
        Task<BusinessEntity.Settings.User?> FindByUserNameAndPassword(string? UserName = null, string? Password = null);
        Task<IEnumerable<BusinessEntity.Settings.User>> GetAll();
        Task<BusinessEntity.Settings.User?> GetById(int id);
        Task<string> Create(int UserId ,BusinessEntity.Settings.User User);
        Task<string> Update(int UserId ,BusinessEntity.Settings.User User);
    }
}
