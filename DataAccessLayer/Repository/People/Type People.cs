using BusinessEntity.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.People
{
    public class TypePeopelRepository : Interface.People.ITypePeopleRepository
    {
        private readonly Database _context;
        private readonly ILogger<TypePeopelRepository> _logger;

        public TypePeopelRepository(Database context, ILogger<TypePeopelRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //******READ*******
        public async Task<IEnumerable<BusinessEntity.People.Type_People>> GetAll()
        {
            _logger.LogInformation("All Type_People have started to be received from the database.");

            var result = await _context.Type_People.ToListAsync();

            _logger.LogInformation("{Count} records received.", result.Count);
            return result;
        }
        public async Task<BusinessEntity.People.Type_People?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Type_People  with ID: {Id}", id);

            var entity = await _context.Type_People.FindAsync(id);

            if (entity == null)
                _logger.LogWarning("Type_People  with ID: {Id} not found", id);
            else
                _logger.LogInformation("Type_People name with ID: {Id} successfully retrieved", id);

            return entity;
        }
        //******CRUD*****
        public async Task<string> Create(int UserId ,BusinessEntity.People.Type_People Type_People)
        {
            if (Type_People == null)
                return "داده ارسال نشده است.";
            try
            {
                _logger.LogInformation("Adding new Type_People: {@Type_People}", Type_People);
                bool nameExists = await _context.Type_People
                    .AsNoTracking()
                    .AnyAsync(i => i.Name == Type_People.Name);

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Type_People name: {Name}", Type_People.Name);
                    return "نام وارد شده تکراری است.";
                }

                var Logs = new BusinessEntity.Settings.LogUser();
                {
                    Logs.Description = $"ثبت نوع اشخاص با نام{Type_People.Name} ";
                    Logs.UserId = UserId;
                    Logs.Date = DateTime.UtcNow;
                }
                _logger.LogInformation("Request to add logs with Async: {Logs}", Logs);
                await _context.LogUser.AddAsync(Logs);
                Type_People.Id = 0;
                await _context.Type_People.AddAsync(Type_People);
                _logger.LogInformation("Tracked entries before SaveChanges: {Count}", _context.ChangeTracker.Entries().Count());

                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Type_People added successfully. ID: {Id}", Type_People.Id);
                    return "عملیات با موفقیت انجام شد.";
                }
                _logger.LogWarning("No changes saved when adding Type_People: {@Type_People}", Type_People);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding Type_People: {@Type_People}", Type_People);
                return "خطایی در ذخیره اطلاعات رخ داد. لطفاً داده‌ها را بررسی کنید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding Type_People: {@Type_People}", Type_People);
                return "خطای غیرمنتظره رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
        public async Task<string> Update(int UserId, BusinessEntity.People.Type_People Type_People)
        {
            try
            {
                _logger.LogInformation("Request update for Type_People: {@Type_People}", Type_People);

                if (Type_People == null)
                {
                    _logger.LogWarning("Null Type_People submitted.");
                    throw new ArgumentNullException(nameof(Type_People));
                }

                bool nameExists = await _context.Type_People
                    .AnyAsync(i => i.Name == Type_People.Name && i.Id != Type_People.Id);

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Type_People name: {Name}", Type_People.Name);
                    return "نام وارد شده تکراری است.";
                }

                var existing = await _context.Type_People.FindAsync(Type_People.Id);

                if (existing == null)
                {
                    _logger.LogWarning("Type_People with ID: {Id} not found.", Type_People.Id);
                    return "شناسه وارد شده با شناسه ذخیره در سیستم مطابقت ندارد.";
                }

                var Logs = new BusinessEntity.Settings.LogUser();
                {
                    Logs.Description = $"ویرایش گروه اشخاص با نام{Type_People.Name} ";
                    Logs.UserId = UserId;
                    Logs.Date = DateTime.UtcNow;
                }
                _logger.LogInformation("Request to add logs with Async: {Logs}", Logs);
                await _context.LogUser.AddAsync(Logs);
                _context.Entry(existing).CurrentValues.SetValues(Type_People);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Type_People with ID: {Id} successfully updated", Type_People.Id);
                    return "عملیات با موفقیت انجام شد.";
                }

                _logger.LogWarning("No changes detected for Type_People with ID: {Id}", Type_People.Id);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating Type_People with ID: {Id}", Type_People?.Id);
                return "این رکورد توسط کاربر دیگری تغییر یافته است. لطفاً صفحه را رفرش کنید و مجدد تلاش کنید.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating Type_People with ID: {Id}", Type_People?.Id);
                return "خطایی در ذخیره تغییرات رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating Type_People");
                return "خطای غیرمنتظره رخ داد. لطفاً مجدداً تلاش کنید.";
            }
        }
        public async Task<string> Delete(int userId, int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("درخواست حذف نوع اشخاص با ID: {Id}", id);
                var entity = await _context.Type_People.FindAsync(id);

                if (entity == null)
                    return "نوع اشخاص مورد نظر یافت نشد.";

                if (entity.IsDelete == false)
                    return "امکان حذف این نوع اشخاصا وجود ندارد.";

                bool hasRelatedAccounts = await _context.People.AnyAsync(a => a.TypePeopleId == id);
                if (hasRelatedAccounts)
                    return "امکان حذف این نوع اشخاص به دلیل وجود حساب‌های وابسته وجود ندارد.";

                // ثبت لاگ
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"حذف نوع اشخاص با نام {entity.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                _context.Type_People.Remove(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("نوع اشخاص با موفقیت حذف شد. ID: {Id}", id);
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در حذف نوع اشخاص  با ID: {Id}", id);
                return "خطایی در عملیات حذف رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
    }
}
