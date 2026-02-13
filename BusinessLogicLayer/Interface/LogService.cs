

namespace BusinessLogicLayer.Interface
{
    public interface ILogService
    {
        Task CreateLogAsync(string description, int userId);
    }

}
