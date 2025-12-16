using BusinessEntity.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Settings
{
    public class GroupUserRepository : Interface.Settings.IGroupUserRepository
    {
        private readonly Database _context;
        private readonly ILogger<GroupUserRepository> _logger;

        public GroupUserRepository(Database context, ILogger<GroupUserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //******READ*******
        public async Task<IEnumerable<Group_User>> GetAll()
        {
            _logger.LogInformation("All Group_User have started to be received from the database.");
            var result = await _context.Group_User.ToListAsync();
            _logger.LogInformation("{Count} records received.", result.Count);
            return result;
        }
        public async Task<Group_User?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Group_User  with ID: {Id}", id);

            var entity = await _context.Group_User.FindAsync(id);

            if (entity == null)
                _logger.LogWarning("Group_User  with ID: {Id} not found", id);
            else
                _logger.LogInformation("Group_User name with ID: {Id} successfully retrieved", id);

            return entity;
        }
        //******CRUD*****
        public async Task<string> Create(int UserId, BusinessEntity.Settings.Group_User Group_User)
        {
            if (Group_User == null)
                return "داده ارسال نشده است.";
            try
            {
                _logger.LogInformation("Adding new Group_User: {@Group_User}", Group_User);
                bool nameExists = await _context.Group_User
                    .AnyAsync(b => b.Name.Trim().ToLower() == Group_User.Name.Trim().ToLower());

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Group_User name: {Name}", Group_User.Name);
                    return "نام وارد شده تکراری است.";
                }

                var Logs = new BusinessEntity.Settings.LogUser();
                {
                    Logs.Description = $"ثبت گروه کاربری با نام{Group_User.Name} ";
                    Logs.UserId = UserId;
                    Logs.Date = DateTime.UtcNow;
                }
                _logger.LogInformation("Request to add logs with Async: {Logs}", Logs);
                await _context.LogUser.AddAsync(Logs);
                Group_User.Id = 0;
                await _context.Group_User.AddAsync(Group_User);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Group_User added successfully. ID: {Id}", Group_User.Id);
                    return "عملیات با موفقیت انجام شد.";
                }
                _logger.LogWarning("No changes saved when adding Group_User: {@Group_User}", Group_User);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding Group_User: {@Group_User}", Group_User);
                return "خطایی در ذخیره اطلاعات رخ داد. لطفاً داده‌ها را بررسی کنید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding Group_User: {@Group_User}", Group_User);
                return "خطای غیرمنتظره رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
        public async Task<string> Update(int UserId, BusinessEntity.Settings.Group_User Group_User)
        {
            try
            {
                _logger.LogInformation("Request update for Group_User: {@Group_User}", Group_User);

                if (Group_User == null)
                {
                    _logger.LogWarning("Null Group_User submitted.");
                    throw new ArgumentNullException(nameof(Group_User));
                }

                bool nameExists = await _context.Group_User
                    .AnyAsync(i => i.Name == Group_User.Name && i.Id != Group_User.Id);

                if (nameExists)
                {
                    _logger.LogWarning("Duplicate Group_User name: {Name}", Group_User.Name);
                    return "نام وارد شده تکراری است.";
                }

                var existing = await _context.Group_User.FindAsync(Group_User.Id);

                if (existing == null)
                {
                    _logger.LogWarning("Group_User with ID: {Id} not found.", Group_User.Id);
                    return "شناسه وارد شده با شناسه ذخیره در سیستم مطابقت ندارد.";
                }

                var Logs = new BusinessEntity.Settings.LogUser();
                {
                    Logs.Description = $"ویرایش گروه کاربری با نام{Group_User.Name} ";
                    Logs.UserId = UserId;
                    Logs.Date = DateTime.UtcNow;
                }
                _logger.LogInformation("Request to add logs with Async: {Logs}", Logs);
                await _context.LogUser.AddAsync(Logs);
                _context.Entry(existing).CurrentValues.SetValues(Group_User);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Group_User with ID: {Id} successfully updated", Group_User.Id);
                    return "عملیات با موفقیت انجام شد.";
                }

                _logger.LogWarning("No changes detected for Group_User with ID: {Id}", Group_User.Id);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating Group_User with ID: {Id}", Group_User?.Id);
                return "این رکورد توسط کاربر دیگری تغییر یافته است. لطفاً صفحه را رفرش کنید و مجدد تلاش کنید.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating Group_User with ID: {Id}", Group_User?.Id);
                return "خطایی در ذخیره تغییرات رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating Group_User");
                return "خطای غیرمنتظره رخ داد. لطفاً مجدداً تلاش کنید.";
            }
        }
        public async Task<string> Delete(int UserId, int id)
        {
            var group = await _context.Group_User
                .IgnoreQueryFilters() // مهم: اگر قبلاً Soft Delete شده
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
                return "گروه کاربری یافت نشد.";

            if (group.IsDelete)
                return "این گروه قبلاً غیرفعال شده است.";

            bool hasActiveUsers = group.Users.Any(u => !u.IsDelete);
            if (hasActiveUsers)
                return "امکان حذف گروه کاربری به دلیل وجود کاربر فعال وجود ندارد.";

            bool hasAccessLevel = await _context.Access_Level
                .AnyAsync(a => a.GroupUserId == id);
            if (hasAccessLevel)
                return "امکان حذف گروه کاربری به دلیل وجود سطح دسترسی وابسته وجود ندارد.";

            group.IsDelete = true;

            await _context.LogUser.AddAsync(new LogUser
            {
                Description = $"غیرفعال‌سازی گروه کاربری «{group.Name}»",
                UserId = UserId,
                Date = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return "عملیات با موفقیت انجام شد.";
        }

    }
}
