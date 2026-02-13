using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface
{
    public interface IRepository<T> where T : class
    {
        // ========== Get Methods ==========
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(int id,
                              CancellationToken cancellationToken = default,
                              params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default,
                                         params Expression<Func<T, object>>[] includes);

        // ========== Find Methods ==========
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
                                       CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
                                       CancellationToken cancellationToken = default,
                                       params Expression<Func<T, object>>[] includes);

        Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate,
                                      CancellationToken cancellationToken = default);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
                                     CancellationToken cancellationToken = default);

        // ========== Add Methods ==========
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        // ========== Update Methods ==========
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);

        // ========== Remove Methods ==========
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        // ========== Check & Count ==========
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate,
                            CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate,
                             CancellationToken cancellationToken = default);
    }
}
