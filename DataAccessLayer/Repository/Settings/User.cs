using BusinessEntity.Settings;
using DataAccessLayer.Interface.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Settings
{
    public class UserRepository : Interface.Settings.IUserRepository
    {
        private readonly Database _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(Database context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //******READ*******
        public async Task<List<UserComboDto>> GetActiveUsersAsync()
        {
            return await _context.User
                .Where(u => u.IsActive && !u.IsDelete)
                .Include(u => u.People)
                .Select(u => new UserComboDto
                {
                    UserId = u.Id,
                    FullName = u.People.FirstName + " " + u.People.LastName
                })
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }
        public async Task<IEnumerable<User>> GetAll()
        {
            _logger.LogInformation("All v have started to be received from the database.");

            var result = await _context.User.ToListAsync();

            _logger.LogInformation("{Count} records received.", result.Count);
            return result;
        }
        public async Task<User?> GetById(int id)
        {
            _logger.LogInformation("Request to receive User  with ID: {Id}", id);

            var entity = await _context.User.FindAsync(id);

            if (entity == null)
                _logger.LogWarning("User  with ID: {Id} not found", id);
            else
                _logger.LogInformation("User name with ID: {Id} successfully retrieved", id);

            return entity;
        }
        public async Task<User?> FindByUserNameAndPassword(string? userName = null, string? password = null)
        {
            _logger.LogInformation("Request to receive User with UserName: {UserName}", userName);

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password))
                return null;

            var entity = await _context.User
                .Include(u => u.Group_User)
                    .ThenInclude(g => g.AccessLevel)
                .FirstOrDefaultAsync(r => r.UserName == userName);

            if (entity == null)
            {
                _logger.LogWarning("User with UserName: {UserName} not found", userName);
                return null;
            }

            if (!PasswordHasher.Verify(password, entity.Password))
            {
                _logger.LogWarning("Invalid password for UserName: {UserName}", userName);
                return null;
            }

            _logger.LogInformation("User with ID: {Id} successfully retrieved", entity.Id);
            return entity;
        }

        //******CRUD*****
        private DateTime ToUtcSafe(DateTime dt)
        {
            // اگر قبلاً UTC است، برگردان
            if (dt.Kind == DateTimeKind.Utc) return dt;

            // اگر Local است، به UTC تبدیل کن
            if (dt.Kind == DateTimeKind.Local) return dt.ToUniversalTime();

            // اگر Unspecified است، فرض کن مشتری UTC فرستاده (یا اگر می‌خواهی فرض Local بگیری از dt.ToUniversalTime())
            // اینجا ما مشخص می‌کنیم که Unspecified را به UTC تگ می‌کنیم (بدون تبدیل)
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
        }
        public async Task<string> Create(int currentUserId, User user)
        {
            try
            {
                if (user == null) return "درخواست نامعتبر است.";

                // duplicate username check
                bool nameExists = await _context.User
                    .AnyAsync(u => u.UserName.Trim().ToLower() == user.UserName.Trim().ToLower());
                if (nameExists) return "نام کاربری وارد شده تکراری است.";

                // FK checks (مثال)
                bool peopleExists = await _context.People.AnyAsync(p => p.Id == user.PeopleId);
                if (!peopleExists) return "فرد انتخاب شده معتبر نیست.";

                bool groupExists = await _context.Group_User.AnyAsync(g => g.Id == user.GroupUserId);
                if (!groupExists) return "گروه انتخاب شده معتبر نیست.";

                // hash password
                string hash = PasswordHasher.Hash(user.Password ?? string.Empty);

                // Normalize dates to UTC
                DateTime validityUtc = user.Validity == default ? DateTime.UtcNow.AddYears(1) : ToUtcSafe(user.Validity);
                DateTime lastActivityUtc = user.LastActivity == default ? DateTime.UtcNow : ToUtcSafe(user.LastActivity);

                var userEntity = new BusinessEntity.Settings.User
                {
                    UserName = user.UserName.Trim(),
                    Password = hash,
                    PeopleId = user.PeopleId,
                    GroupUserId = user.GroupUserId,
                    IsActive = user.IsActive,
                    IsDelete = false,
                    Validity = validityUtc,
                    LastActivity = lastActivityUtc,
                    ImageAddress = user.ImageAddress
                };

                var log = new BusinessEntity.Settings.LogUser
                {
                    Description = $"ثبت کاربر با نام {userEntity.UserName}",
                    UserId = currentUserId,
                    Date = DateTime.UtcNow // لاگ هم UTC باشه
                };

                await _context.LogUser.AddAsync(log);
                await _context.User.AddAsync(userEntity);

                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _logger.LogInformation("User added. ID: {Id}", userEntity.Id);
                    return "عملیات با موفقیت انجام شد.";
                }
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateException dbEx)
            {
                var sqlMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                _logger.LogError(dbEx, "Database error while adding user: {SqlMessage}", sqlMessage);
                return $"خطایی در ذخیره اطلاعات رخ داد: {sqlMessage}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding user");
                return "خطای غیرمنتظره رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
        public async Task<string> Update(int currentUserId, User dto)
        {
            try
            {
                _logger.LogInformation("Update request for User: {@dto}", dto);

                if (dto == null)
                    return "درخواست نامعتبر است.";

                // بررسی اینکه کاربری با این ID وجود دارد
                var user = await _context.User.FindAsync(dto.Id);

                if (user == null)
                {
                    _logger.LogWarning("User ID {Id} not found.", dto.Id);
                    return "شناسه وارد شده با شناسه ذخیره شده مطابقت ندارد.";
                }

                // چک عدم تکراری بودن یوزرنیم
                bool nameExists = await _context.User
                    .AnyAsync(x => x.UserName.ToLower() == dto.UserName.ToLower() && x.Id != dto.Id);

                if (nameExists)
                {
                    return "نام کاربری وارد شده تکراری است.";
                }

                // چک صحت FK ها
                bool peopleExists = await _context.People.AnyAsync(p => p.Id == dto.PeopleId);
                if (!peopleExists)
                    return "فرد انتخاب شده معتبر نیست.";

                bool groupExists = await _context.Group_User.AnyAsync(g => g.Id == dto.GroupUserId);
                if (!groupExists)
                    return "گروه انتخاب‌شده معتبر نیست.";

                // اعمال تغییرات فقط روی فیلدهای مجاز
                user.UserName = dto.UserName.Trim();
                user.PeopleId = dto.PeopleId;
                user.GroupUserId = dto.GroupUserId;
                user.IsActive = dto.IsActive;
                user.ImageAddress = dto.ImageAddress;

                // تاریخ‌ها باید UTC باشند
                user.Validity = ToUtcSafe(dto.Validity);

                // اگر رمز جدید فرستاده شده (رمز قبلی حفظ نشود)
                if (!string.IsNullOrWhiteSpace(dto.Password))
                {
                    user.Password = PasswordHasher.Hash(dto.Password);
                }

                // لاگ ثبت تغییر
                var log = new LogUser
                {
                    Description = $"ویرایش کاربر {user.UserName}",
                    Date = DateTime.UtcNow,
                    UserId = currentUserId
                };

                await _context.LogUser.AddAsync(log);

                int saved = await _context.SaveChangesAsync();

                if (saved > 0)
                {
                    _logger.LogInformation("User {Id} updated successfully.", user.Id);
                    return "عملیات با موفقیت انجام شد.";
                }

                return "هیچ تغییری انجام نشد.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error for User ID {Id}", dto.Id);
                return "این رکورد همزمان توسط کاربر دیگری ویرایش شده است.";
            }
            catch (DbUpdateException dbEx)
            {
                string msg = dbEx.InnerException?.Message ?? dbEx.Message;
                _logger.LogError(dbEx, "Database error updating User: {Message}", msg);
                return $"خطایی در ذخیره اطلاعات رخ داد: {msg}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error on update User.");
                return "خطای غیرمنتظره رخ داد.";
            }
        }
    }
}
