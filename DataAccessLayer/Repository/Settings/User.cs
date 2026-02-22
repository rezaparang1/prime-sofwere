using BusinessEntity.Settings;
using DataAccessLayer.Interface.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace DataAccessLayer.Repository.Settings
{
    public class UserRepository : IUserRepository
    {
        private readonly Database _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(Database context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ***** GetActiveUsersAsync *****
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

        // ***** GetAll *****
        public async Task<IEnumerable<BusinessEntity.DTO.Settings.UserDto>> GetAll()
        {
            var users = await _context.User
                .Include(u => u.People)
                .Include(u => u.Group_User)
                .Where(u => !u.IsDelete)
                .Select(u => new BusinessEntity.DTO.Settings.UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    PeopleId = u.PeopleId,
                    PeopleFullName = u.People != null ? u.People.FirstName + " " + u.People.LastName : null,
                    GroupUserId = u.GroupUserId,
                    GroupName = u.Group_User != null ? u.Group_User.Name : null,
                    IsActive = u.IsActive,
                    LastActivity = u.LastActivity,
                    Validity = u.Validity
                })
                .OrderBy(u => u.UserName)
                .ToListAsync();

            return users;
        }

        // ***** GetById *****
        public async Task<User?> GetById(int id)
        {
            return await _context.User
                .Include(u => u.People)
                .Include(u => u.Group_User)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDelete);
        }

        // ***** FindByUserNameAndPassword (برای لاگین) - بدون تغییر *****
        public async Task<User?> FindByUserNameAndPassword(string? userName = null, string? password = null)
        {
            _logger.LogInformation("Request to receive User with UserName: {UserName}", userName);

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password))
                return null;

            var entity = await _context.User
                .Include(u => u.Group_User)
                    .ThenInclude(g => g.AccessLevel)
                .FirstOrDefaultAsync(r => r.UserName == userName && !r.IsDelete);

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

        // ***** Create *****
        public async Task<Result> Create(User user)
        {
            try
            {
                // بررسی تکراری بودن نام کاربری
                bool nameExists = await _context.User
                    .AnyAsync(u => u.UserName.Trim().ToLower() == user.UserName.Trim().ToLower() && !u.IsDelete);

                if (nameExists)
                    return Result.Failure("نام کاربری وارد شده تکراری است.");

                // بررسی وجود شخص
                bool peopleExists = await _context.People
                    .AnyAsync(p => p.Id == user.PeopleId && !p.IsDelete);

                if (!peopleExists)
                    return Result.Failure("فرد انتخاب شده معتبر نیست.");

                // بررسی وجود گروه کاربری
                bool groupExists = await _context.Group_User
                    .AnyAsync(g => g.Id == user.GroupUserId);

                if (!groupExists)
                    return Result.Failure("گروه انتخاب شده معتبر نیست.");

                // هش کردن رمز عبور
                string hash = PasswordHasher.Hash(user.Password ?? string.Empty);

                // نرمال‌سازی تاریخ‌ها به UTC
                DateTime validityUtc = user.Validity == default ? DateTime.UtcNow.AddYears(1) : ToUtcSafe(user.Validity);
                DateTime lastActivityUtc = DateTime.UtcNow;

                user.Password = hash;
                user.Validity = validityUtc;
                user.LastActivity = lastActivityUtc;
                user.IsDelete = false;
                user.CurrentSessionId = null;

                await _context.User.AddAsync(user);

                return Result.Success("عملیات با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating User: {@User}", user);
                return Result.Failure($"خطا در ایجاد کاربر: {ex.Message}");
            }
        }

        // ***** Update *****
        public async Task<Result> Update(User user)
        {
            try
            {
                // بررسی وجود کاربر
                var existingUser = await _context.User
                    .FirstOrDefaultAsync(u => u.Id == user.Id && !u.IsDelete);

                if (existingUser == null)
                    return Result.Failure("کاربر یافت نشد.");

                // بررسی تکراری بودن نام کاربری (به جز خودش)
                bool nameExists = await _context.User
                    .AnyAsync(u => u.Id != user.Id &&
                                  u.UserName.Trim().ToLower() == user.UserName.Trim().ToLower() &&
                                  !u.IsDelete);

                if (nameExists)
                    return Result.Failure("نام کاربری وارد شده تکراری است.");

                // بررسی وجود شخص
                bool peopleExists = await _context.People
                    .AnyAsync(p => p.Id == user.PeopleId && !p.IsDelete);

                if (!peopleExists)
                    return Result.Failure("فرد انتخاب شده معتبر نیست.");

                // بررسی وجود گروه کاربری
                bool groupExists = await _context.Group_User
                    .AnyAsync(g => g.Id == user.GroupUserId);

                if (!groupExists)
                    return Result.Failure("گروه انتخاب‌شده معتبر نیست.");

                // به‌روزرسانی فیلدها
                existingUser.UserName = user.UserName.Trim();
                existingUser.PeopleId = user.PeopleId;
                existingUser.GroupUserId = user.GroupUserId;
                existingUser.IsActive = user.IsActive;
                existingUser.ImageAddress = user.ImageAddress;
                existingUser.Validity = ToUtcSafe(user.Validity);

                // اگر رمز عبور جدید ارسال شده باشد
                if (!string.IsNullOrWhiteSpace(user.Password))
                {
                    existingUser.Password = PasswordHasher.Hash(user.Password);
                }

                return Result.Success("عملیات با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating User: {@User}", user);
                return Result.Failure($"خطا در به‌روزرسانی کاربر: {ex.Message}");
            }
        }

        // ***** Delete *****
        public async Task<Result> Delete(int id)
        {
            try
            {
                var user = await _context.User
                    .FirstOrDefaultAsync(u => u.Id == id && !u.IsDelete);

                if (user == null)
                    return Result.Failure("کاربر یافت نشد.");

                // Soft Delete
                user.IsDelete = true;
                user.IsActive = false;
                user.CurrentSessionId = null;

                return Result.Success("کاربر با موفقیت حذف شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting User with ID: {Id}", id);
                return Result.Failure($"خطا در حذف کاربر: {ex.Message}");
            }
        }

        // ***** کمکی: تبدیل تاریخ به UTC *****
        private DateTime ToUtcSafe(DateTime dt)
        {
            if (dt.Kind == DateTimeKind.Utc) return dt;
            if (dt.Kind == DateTimeKind.Local) return dt.ToUniversalTime();
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
        }
    }
}
