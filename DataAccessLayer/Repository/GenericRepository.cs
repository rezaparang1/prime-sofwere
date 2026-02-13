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
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly Database _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(Database context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<T> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task<Result> Add(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                return Result.Success("عملیات با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"خطا در انجام عملیات: {ex.Message}");
            }
        }

        public Task<Result> Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                return Task.FromResult(Result.Success("عملیات با موفقیت انجام شد."));
            }
            catch (Exception ex)
            {
                return Task.FromResult(Result.Failure($"خطا در انجام عملیات: {ex.Message}"));
            }
        }

        public Task<Result> Delete(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                return Task.FromResult(Result.Success("عملیات با موفقیت انجام شد."));
            }
            catch (Exception ex)
            {
                return Task.FromResult(Result.Failure($"خطا در انجام عملیات: {ex.Message}"));
            }
        }

        public async Task SaveAsync()
            => await _context.SaveChangesAsync();
    }
}
