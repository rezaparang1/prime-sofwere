using DataAccessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly Database _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(Database context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // ========== Get Methods ==========
        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<T?> GetByIdAsync(int id,
                                           CancellationToken cancellationToken = default,
                                           params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default,
                                                      params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync(cancellationToken);
        }

        // ========== Find Methods ==========
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
                                                    CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
                                                    CancellationToken cancellationToken = default,
                                                    params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
                query = query.Include(include);

            return await query.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate,
                                                   CancellationToken cancellationToken = default)
        {
            return await _dbSet.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
                                                  CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        // ========== Add Methods ==========
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        // ========== Update Methods ==========
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        // ========== Remove Methods ==========
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        // ========== Check & Count ==========
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate,
                                         CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate,
                                          CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(predicate, cancellationToken);
        }
    }
}
