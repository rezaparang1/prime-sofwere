using BusinessLogicLayer.Interface;
using DataAccessLayer;
using DataAccessLayer.Interface;
using Npgsql.Replication.PgOutput.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IRepository<T> _repo;
        private readonly ILogService _logService;
        private readonly Database _context;

        public GenericService(
            IRepository<T> repo,
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
                await _repo.AddAsync(entity);
                // ذخیره‌سازی نهایی (بعد از لاگ) انجام می‌شود، پس هنوز Save نکنید.
                await _logService.CreateLogAsync(logText, userId);
                await _context.SaveChangesAsync(); // همه تغییرات در یک تراکنش ذخیره می‌شوند
                await transaction.CommitAsync();
                return Result.Success("ثبت انجام شد و لاگ ذخیره شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                return Result.Failure($"خطا در انجام عملیات: {innerMessage}");
            }
        }
        public async Task<Result> UpdateWithLogAsync(T entity, string log, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _repo.Update(entity);                         // ✅ متد همزمان Update
                await _context.SaveChangesAsync();            // ✅ ذخیره تغییرات
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
                _repo.Remove(entity);                          // ✅ استفاده از Remove
                await _context.SaveChangesAsync();             // ✅ ذخیره تغییرات
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
