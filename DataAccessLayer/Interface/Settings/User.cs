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
        Task<User?> FindByUserNameAndPassword(string? userName = null, string? password = null);
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(int id);
        Task<Result> Create(User user);
        Task<Result> Update(User user);
        Task<Result> Delete(int id);
    }
}
