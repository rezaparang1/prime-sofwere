using BusinessEntity.Settings;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Settings
{
    public interface IUserService
    {
        Task<List<UserComboDto>> GetActiveUsersAsync();
        Task<User?> FindByUserNameAndPassword(string? userName = null, string? password = null);
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(int id);
        Task<Result> Create(User user, int currentUserId);
        Task<Result> Update(User user, int currentUserId);
        Task<Result> Delete(int id, int currentUserId);
    }
}
