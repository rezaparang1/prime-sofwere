using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.People
{
    public class GroupPeopelRepository : Interface.People.IGroupPeopleRepository
    {
        private readonly Database _context;
        private readonly ILogger<GroupPeopelRepository> _logger;

        public GroupPeopelRepository(Database context, ILogger<GroupPeopelRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //******READ*******
        public async Task<IEnumerable<BusinessEntity.People.Group_People>> GetAll()
        {
            _logger.LogInformation("All Group_People have started to be received from the database.");

            var result = await _context.Group_People.ToListAsync();

            _logger.LogInformation("{Count} records received.", result.Count);
            return result;
        }
        public async Task<BusinessEntity.People.Group_People?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Group_People  with ID: {Id}", id);

            var entity = await _context.Group_People.FindAsync(id);

            if (entity == null)
                _logger.LogWarning("Group_People  with ID: {Id} not found", id);
            else
                _logger.LogInformation("Group_People name with ID: {Id} successfully retrieved", id);

            return entity;
        }
        //******CRUD*****
        public async Task<string> Create(int userId, BusinessEntity.People.Group_People Group_People)
        {
            if (Group_People == null)
                return "داده ارسال نشده است.";
            try
            {
                _logger.LogInformation("Starting creation of Group_People: {@Group_People}", Group_People);

                bool nameExists = await _context.Group_People
                    .AnyAsync(b => b.Name.Trim().ToLower() == Group_People.Name.Trim().ToLower());
                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Group_People name: {Name}", Group_People.Name);
                    return "نام وارد شده تکراری است.";
                }

                var log = new BusinessEntity.Settings.LogUser
                {
                    Description = $"ثبت گروه اشخاص با نام {Group_People.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                };

                await _context.LogUser.AddAsync(log);

                Group_People.Id = 0;
                await _context.Group_People.AddAsync(Group_People);

                _logger.LogInformation("Tracked entries before SaveChanges: {Count}", _context.ChangeTracker.Entries().Count());

                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Group_People created successfully with ID: {Id}", Group_People.Id);
                    return $"عملیات با موفقیت انجام شد. ";
                }

                _logger.LogWarning("No changes were saved for Group_People: {@Group_People}", Group_People);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while creating Group_People: {@Group_People}", Group_People);
                return "خطا در ذخیره‌سازی اطلاعات رخ داد. لطفاً داده‌ها را بررسی کنید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating Group_People: {@Group_People}", Group_People);
                return "خطای غیرمنتظره رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
        public async Task<string> Update(int UserId, BusinessEntity.People.Group_People Group_People)
        {
            try
            {
                _logger.LogInformation("Request update for Group_People: {@Group_People}", Group_People);

                if (Group_People == null)
                {
                    _logger.LogWarning("Null Group_People submitted.");
                    throw new ArgumentNullException(nameof(Group_People));
                }

                bool nameExists = await _context.Group_People
                    .AnyAsync(i => i.Name == Group_People.Name && i.Id != Group_People.Id);

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Group_People name: {Name}", Group_People.Name);
                    return "نام وارد شده تکراری است.";
                }

                var existing = await _context.Group_People.FindAsync(Group_People.Id);

                if (existing == null)
                {
                    _logger.LogWarning("Group_People with ID: {Id} not found.", Group_People.Id);
                    return "شناسه وارد شده با شناسه ذخیره در سیستم مطابقت ندارد.";
                }

                var Logs = new BusinessEntity.Settings.LogUser();
                {
                    Logs.Description = $"ویرایش گروه اشخاص با نام{Group_People.Name} ";
                    Logs.UserId = UserId;
                    Logs.Date = DateTime.UtcNow;
                }
                _logger.LogInformation("Request to add logs with Async: {Logs}", Logs);
                await _context.LogUser.AddAsync(Logs);
                _context.Entry(existing).CurrentValues.SetValues(Group_People);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Group_People with ID: {Id} successfully updated", Group_People.Id);
                    return "عملیات با موفقیت انجام شد.";
                }

                _logger.LogWarning("No changes detected for Group_People with ID: {Id}", Group_People.Id);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating Group_People with ID: {Id}", Group_People?.Id);
                return "این رکورد توسط کاربر دیگری تغییر یافته است. لطفاً صفحه را رفرش کنید و مجدد تلاش کنید.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating Group_People with ID: {Id}", Group_People?.Id);
                return "خطایی در ذخیره تغییرات رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating Group_People");
                return "خطای غیرمنتظره رخ داد. لطفاً مجدداً تلاش کنید.";
            }
        }
        public async Task<string> Delete(int userId, int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("درخواست حذف گروه اشخاص با ID: {Id}", id);
                var entity = await _context.Group_People.FindAsync(id);

                if (entity == null)
                    return "گروه اشخاص مورد نظر یافت نشد.";

                if (entity.IsDelete == false)
                    return "امکان حذف این گروه اشخاصا وجود ندارد.";

                bool hasRelatedAccounts = await _context.People.AnyAsync(a => a.GroupPeopleId == id);
                if (hasRelatedAccounts)
                    return "امکان حذف این گروه اشخاص به دلیل وجود حساب‌های وابسته وجود ندارد.";

                // ثبت لاگ
                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"حذف گروه اشخاص با نام {entity.Name}",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                _context.Group_People.Remove(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("گروه اشخاص با موفقیت حذف شد. ID: {Id}", id);
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در حذف گروه اشخاص  با ID: {Id}", id);
                return "خطایی در عملیات حذف رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
    }
}
