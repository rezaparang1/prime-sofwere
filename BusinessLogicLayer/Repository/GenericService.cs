using BusinessLogicLayer.Interface;
using DataAccessLayer;
using DataAccessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repo;
        private readonly ILogService _logService;
        private readonly Database _context;

        public GenericService(
            IGenericRepository<T> repo,
            ILogService logService,
            Database context)
        {
            _repo = repo;
            _logService = logService;
            _context = context;
        }

        public async Task<Result> AddWithLogAsync(T entity, string logText, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var addResult = await _repo.Add(entity);
                if (!addResult.IsSuccess)
                    return addResult;

                await _repo.SaveAsync();
                await _logService.CreateLogAsync(logText, userId);

                await transaction.CommitAsync();
                return Result.Success("ثبت انجام شد و لاگ ذخیره شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure($"خطا در انجام عملیات: {ex.Message}");
            }
        }

        public async Task<Result> UpdateWithLogAsync(T entity, string log, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var updateResult = await _repo.Update(entity);
                if (!updateResult.IsSuccess)
                    return updateResult;

                await _repo.SaveAsync();
                await _logService.CreateLogAsync(log, userId);

                await transaction.CommitAsync();
                return Result.Success("عملیات با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure($"خطا در انجام عملیات: {ex.Message}");
            }
        }

        public async Task<Result> DeleteWithLogAsync(T entity, string logText, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var delResult = await _repo.Delete(entity);
                if (!delResult.IsSuccess)
                    return delResult;

                await _repo.SaveAsync();
                await _logService.CreateLogAsync(logText, userId);

                await transaction.CommitAsync();
                return Result.Success("حذف انجام شد و لاگ ثبت شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure($"خطا در انجام عملیات: {ex.Message}");
            }
        }
    }
}
