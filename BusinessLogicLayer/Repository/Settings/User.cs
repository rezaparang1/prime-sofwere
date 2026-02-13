using BusinessEntity.Settings;
using BusinessLogicLayer.Interface;
using BusinessLogicLayer.Interface.Settings;
using DataAccessLayer;
using DataAccessLayer.Interface.Settings;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository.Settings
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogService _logService;
        private readonly Database _context;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            ILogService logService,
            Database context,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logService = logService;
            _context = context;
            _logger = logger;
        }

        public async Task<List<UserComboDto>> GetActiveUsersAsync()
        {
            return await _userRepository.GetActiveUsersAsync();
        }

        public async Task<User?> FindByUserNameAndPassword(string? userName = null, string? password = null)
        {
            return await _userRepository.FindByUserNameAndPassword(userName, password);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User?> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<Result> Create(User user, int currentUserId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // اعتبارسنجی
                if (string.IsNullOrWhiteSpace(user.UserName))
                    return Result.Failure("نام کاربری الزامی است.");

                if (string.IsNullOrWhiteSpace(user.Password))
                    return Result.Failure("رمز عبور الزامی است.");

                if (user.Password.Length < 6)
                    return Result.Failure("رمز عبور باید حداقل ۶ کاراکتر باشد.");

                if (user.PeopleId <= 0)
                    return Result.Failure("شخص باید انتخاب شود.");

                if (user.GroupUserId <= 0)
                    return Result.Failure("گروه کاربری باید انتخاب شود.");

                // ایجاد کاربر
                var result = await _userRepository.Create(user);
                if (!result.IsSuccess)
                    return result;

                // ذخیره تغییرات
                await _context.SaveChangesAsync();

                // ثبت لاگ
                await _logService.CreateLogAsync(
                    $"ایجاد کاربر جدید: {user.UserName} (شناسه: {user.Id})",
                    currentUserId);

                await transaction.CommitAsync();
                return Result.Success("کاربر با موفقیت ایجاد شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در ایجاد کاربر: {@User}", user);
                return Result.Failure($"خطا در ایجاد کاربر: {ex.Message}");
            }
        }

        public async Task<Result> Update(User user, int currentUserId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // اعتبارسنجی
                if (string.IsNullOrWhiteSpace(user.UserName))
                    return Result.Failure("نام کاربری الزامی است.");

                if (user.PeopleId <= 0)
                    return Result.Failure("شخص باید انتخاب شود.");

                if (user.GroupUserId <= 0)
                    return Result.Failure("گروه کاربری باید انتخاب شود.");

                // اگر رمز عبور جدید داده شده
                if (!string.IsNullOrWhiteSpace(user.Password) && user.Password.Length < 6)
                    return Result.Failure("رمز عبور جدید باید حداقل ۶ کاراکتر باشد.");

                // به‌روزرسانی کاربر
                var result = await _userRepository.Update(user);
                if (!result.IsSuccess)
                    return result;

                // ذخیره تغییرات
                await _context.SaveChangesAsync();

                // ثبت لاگ
                await _logService.CreateLogAsync(
                    $"به‌روزرسانی کاربر: {user.UserName} (شناسه: {user.Id})",
                    currentUserId);

                await transaction.CommitAsync();
                return Result.Success("کاربر با موفقیت به‌روزرسانی شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در به‌روزرسانی کاربر: {@User}", user);
                return Result.Failure($"خطا در به‌روزرسانی کاربر: {ex.Message}");
            }
        }

        public async Task<Result> Delete(int id, int currentUserId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // دریافت اطلاعات کاربر برای لاگ
                var user = await _userRepository.GetById(id);
                if (user == null)
                    return Result.Failure("کاربر یافت نشد.");

                // بررسی اینکه کاربر جاری خودش را حذف نکند
                if (user.Id == currentUserId)
                    return Result.Failure("شما نمی‌توانید حساب کاربری خود را حذف کنید.");

                // حذف کاربر
                var result = await _userRepository.Delete(id);
                if (!result.IsSuccess)
                    return result;

                // ذخیره تغییرات
                await _context.SaveChangesAsync();

                // ثبت لاگ
                await _logService.CreateLogAsync(
                    $"حذف کاربر: {user.UserName} (شناسه: {id})",
                    currentUserId);

                await transaction.CommitAsync();
                return Result.Success("کاربر با موفقیت حذف شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در حذف کاربر با شناسه: {Id}", id);
                return Result.Failure($"خطا در حذف کاربر: {ex.Message}");
            }
        }
    }
}
