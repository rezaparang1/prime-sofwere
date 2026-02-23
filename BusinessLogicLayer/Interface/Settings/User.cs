using BusinessEntity.Settings;
using BusinessLogicLayer.DTO;


namespace BusinessLogicLayer.Interface.Settings
{
    public interface IUserService
    {
        Task<User?> GetUserEntityByIdAsync(int id);
        Task<List<UserComboDto>> GetActiveUsers();
        Task<User?> FindByUserNameAndPassword(string? userName, string? password);
        Task<IEnumerable<UserDto>> GetAll();
        Task<UserDto?> GetById(int id);
        Task<Result> Create(UserCreateDto dto, int currentUserId);
        Task<Result> Update(int id, UserUpdateDto dto, int currentUserId);
        Task<Result> Delete(int id, int currentUserId);
    }
}
