using BusinessEntity.Settings;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface;
using BusinessLogicLayer.Interface.Settings;
using DataAccessLayer.Interface;
using DataAccessLayer.Repository;
using FluentValidation;
using Microsoft.Extensions.Logging;


namespace BusinessLogicLayer.Repository.Settings
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _logService;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, ILogService logService, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _logService = logService;
            _logger = logger;
        }

        // دریافت موجودیت کامل کاربر با Include (برای استفاده در Auth)
        public async Task<User?> GetUserEntityByIdAsync(int id)
        {
            return await _unitOfWork.Users.GetByIdAsync(
                id,
                default,
                u => u.Group_User,
                u => u.Group_User.AccessLevel);
        }

        // لیست کاربران فعال برای کامبو باکس
        public async Task<List<UserComboDto>> GetActiveUsers()
        {
            try
            {
                var users = await _unitOfWork.Users.FindAsync(u => u.IsActive && !u.IsDelete);
                return users.Select(u => new UserComboDto
                {
                    UserId = u.Id,
                    FullName = u.People != null ? $"{u.People.FirstName} {u.People.LastName}" : null
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت کاربران فعال");
                return new List<UserComboDto>(); // یا throw; بسته به سناریو
            }
        }

        // جستجوی کاربر برای لاگین (با Include)
        public async Task<User?> FindByUserNameAndPassword(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                return null;

            try
            {
                var users = await _unitOfWork.Users.FindAsync(
                    u => u.UserName == userName && !u.IsDelete,
                    default,
                    u => u.Group_User,
                    u => u.Group_User.AccessLevel);

                var user = users.FirstOrDefault();

                if (user == null || !PasswordHasher.Verify(password, user.Password))
                    return null;

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در جستجوی کاربر برای لاگین با نام {UserName}", userName);
                return null;
            }
        }

        // دریافت همه کاربران (تبدیل به DTO)
        public async Task<IEnumerable<UserDto>> GetAll()
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllAsync(default, u => u.People, u => u.Group_User);
                return users.Where(u => !u.IsDelete).Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در GetAll کاربران");
                // در صورت تمایل می‌توانید Exception را دوباره پرتاب کنید یا یک لیست خالی برگردانید
                // اینجا بسته به نیاز پروژه تصمیم بگیرید. پیشنهاد: پرتاب خطا برای مدیریت متمرکز
                throw;
            }
        }

        // دریافت یک کاربر با شناسه
        public async Task<UserDto?> GetById(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id, default, u => u.People, u => u.Group_User);
                return user == null || user.IsDelete ? null : MapToDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت کاربر با شناسه {Id}", id);
                throw;
            }
        }

        // ایجاد کاربر جدید
        public async Task<Result> Create(UserCreateDto dto, int currentUserId)
        {
            // اعتبارسنجی اولیه
            if (string.IsNullOrWhiteSpace(dto.UserName))
                return Result.Failure("نام کاربری الزامی است.");
            if (string.IsNullOrWhiteSpace(dto.Password))
                return Result.Failure("رمز عبور الزامی است.");
            if (dto.Password.Length < 6)
                return Result.Failure("رمز عبور باید حداقل ۶ کاراکتر باشد.");
            if (dto.PeopleId <= 0)
                return Result.Failure("شخص باید انتخاب شود.");
            if (dto.GroupUserId <= 0)
                return Result.Failure("گروه کاربری باید انتخاب شود.");

            try
            {
                // بررسی تکراری نبودن نام کاربری
                var existing = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName);
                if (existing != null)
                    return Result.Failure("این نام کاربری قبلاً ثبت شده است.");

                // ایجاد کاربر
                var user = new User
                {
                    UserName = dto.UserName.Trim(),
                    Password = PasswordHasher.Hash(dto.Password),
                    PeopleId = dto.PeopleId,
                    GroupUserId = dto.GroupUserId,
                    IsActive = dto.IsActive,
                    Validity = dto.Validity?.ToUniversalTime() ?? DateTime.UtcNow.AddYears(1),
                    ImageAddress = dto.ImageAddress,
                    LastActivity = DateTime.UtcNow,
                    IsDelete = false
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                await _logService.CreateLogAsync($"ایجاد کاربر جدید: {user.UserName} (شناسه {user.Id})", currentUserId);
                return Result.Success("کاربر با موفقیت ایجاد شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ایجاد کاربر");
                return Result.Failure($"خطا در ایجاد کاربر: {ex.Message}");
            }
        }

        // ویرایش کاربر
        public async Task<Result> Update(int id, UserUpdateDto dto, int currentUserId)
        {
            try
            {
                var existingUser = await _unitOfWork.Users.GetByIdAsync(id);
                if (existingUser == null || existingUser.IsDelete)
                    return Result.Failure("کاربر یافت نشد.");

                // اعتبارسنجی نام کاربری (اگر تغییر کرده باشد)
                if (!string.IsNullOrWhiteSpace(dto.UserName) && dto.UserName != existingUser.UserName)
                {
                    var duplicate = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName);
                    if (duplicate != null)
                        return Result.Failure("این نام کاربری قبلاً ثبت شده است.");
                    existingUser.UserName = dto.UserName.Trim();
                }

                // به‌روزرسانی فیلدها
                if (dto.PeopleId.HasValue)
                    existingUser.PeopleId = dto.PeopleId.Value;
                if (dto.GroupUserId.HasValue)
                    existingUser.GroupUserId = dto.GroupUserId.Value;
                if (dto.IsActive.HasValue)
                    existingUser.IsActive = dto.IsActive.Value;
                if (!string.IsNullOrWhiteSpace(dto.ImageAddress))
                    existingUser.ImageAddress = dto.ImageAddress;
                if (dto.Validity.HasValue)
                    existingUser.Validity = dto.Validity.Value.ToUniversalTime();

                // اگر رمز عبور جدید داده شده، هش کن
                if (!string.IsNullOrWhiteSpace(dto.Password))
                {
                    if (dto.Password.Length < 6)
                        return Result.Failure("رمز عبور جدید باید حداقل ۶ کاراکتر باشد.");
                    existingUser.Password = PasswordHasher.Hash(dto.Password);
                }

                _unitOfWork.Users.Update(existingUser);
                await _unitOfWork.SaveChangesAsync();

                await _logService.CreateLogAsync($"ویرایش کاربر {existingUser.UserName} (شناسه {id})", currentUserId);
                return Result.Success("کاربر با موفقیت به‌روزرسانی شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ویرایش کاربر با شناسه {Id}", id);
                return Result.Failure($"خطا در ویرایش کاربر: {ex.Message}");
            }
        }

        // حذف کاربر (Soft Delete)
        public async Task<Result> Delete(int id, int currentUserId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null || user.IsDelete)
                    return Result.Failure("کاربر یافت نشد.");
                if (user.Id == currentUserId)
                    return Result.Failure("شما نمی‌توانید حساب کاربری خود را حذف کنید.");

                user.IsDelete = true;
                user.IsActive = false;
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();

                await _logService.CreateLogAsync($"حذف کاربر {user.UserName} (شناسه {id})", currentUserId);
                return Result.Success("کاربر با موفقیت حذف شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در حذف کاربر با شناسه {Id}", id);
                return Result.Failure($"خطا در حذف کاربر: {ex.Message}");
            }
        }

        // تبدیل موجودیت User به DTO (امن در برابر null)
        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                PeopleId = user.PeopleId,
                PeopleFullName = user.People != null ? $"{user.People.FirstName} {user.People.LastName}" : null,
                GroupUserId = user.GroupUserId,
                GroupName = user.Group_User?.Name,
                IsActive = user.IsActive,
                LastActivity = user.LastActivity,
                Validity = user.Validity,
                ImageAddress = user.ImageAddress
            };
        }
    }
}
