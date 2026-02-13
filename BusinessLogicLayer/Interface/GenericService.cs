using DataAccessLayer;


namespace BusinessLogicLayer.Interface
{
    public interface IGenericService<T> where T : class
    {
        Task<Result> AddWithLogAsync(T entity, string logText, int userId);
        Task<Result> UpdateWithLogAsync(T entity, string log, int userId);
        Task<Result> DeleteWithLogAsync(T entity, string logText, int userId);
    }
}
