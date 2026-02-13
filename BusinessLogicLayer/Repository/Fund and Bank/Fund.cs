using BusinessEntity.DTO.Fund;
using BusinessLogicLayer.Interface;
using BusinessLogicLayer.Interface.Fund;
using DataAccessLayer;
using DataAccessLayer.Interface.Fund;
using BusinessEntity.Fund;

namespace BusinessLogicLayer.Repository.Fund
{
    public class FundService : IFundService
    {
        private readonly IFundRepository _fundRepository;
        private readonly ILogService _logService;
        private readonly Database _context;

        public FundService(
            IFundRepository fundRepository,
            ILogService logService,
            Database context)
        {
            _fundRepository = fundRepository;
            _logService = logService;
            _context = context;
        }

        public async Task<List<BusinessEntity.Fund.Fund>> Search(string? name = null)
        {
            return await _fundRepository.Search(name);
        }

        public async Task<List<InventoryItemDto>> GetInventoryDetails()
        {
            return await _fundRepository.GetInventoryDetails();
        }

        public async Task<IEnumerable<BusinessEntity.Fund.Fund>> GetAll()
        {
            return await _fundRepository.GetAll();
        }

        public async Task<BusinessEntity.Fund.Fund?> GetById(int id)
        {
            return await _fundRepository.GetById(id);
        }

        public async Task<Result> Create(BusinessEntity.Fund.Fund fund, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // اعتبارسنجی
                if (string.IsNullOrWhiteSpace(fund.Name))
                    return Result.Failure("نام صندوق نمی‌تواند خالی باشد.");

                if (fund.Inventory < 0)
                    return Result.Failure("موجودی اولیه نمی‌تواند منفی باشد.");

                // ایجاد صندوق
                var dalResult = await _fundRepository.Create(fund);
                if (!dalResult.IsSuccess)
                    return Result.Failure(dalResult.Message);   // ✅ تبدیل به BLL Result

                // ذخیره تغییرات
                await _context.SaveChangesAsync();

                // ثبت لاگ
                string logText = $"ثبت صندوق جدید با نام '{fund.Name}' و موجودی {fund.Inventory:#,##0}";
                await _logService.CreateLogAsync(logText, userId);

                await transaction.CommitAsync();
                return Result.Success("صندوق با موفقیت ایجاد شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure($"خطا در ایجاد صندوق: {ex.Message}");
            }
        }

        public async Task<Result> Update(BusinessEntity.Fund.Fund fund, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // اعتبارسنجی
                if (string.IsNullOrWhiteSpace(fund.Name))
                    return Result.Failure("نام صندوق نمی‌تواند خالی باشد.");

                if (fund.Inventory < 0)
                    return Result.Failure("موجودی نمی‌تواند منفی باشد.");

                // بروزرسانی صندوق
                var dalResult = await _fundRepository.Update(fund);
                if (!dalResult.IsSuccess)
                    return Result.Failure(dalResult.Message);   // ✅ تبدیل به BLL Result

                // ذخیره تغییرات
                await _context.SaveChangesAsync();

                // ثبت لاگ
                string logText = $"ویرایش صندوق با شناسه {fund.Id} به نام '{fund.Name}'";
                await _logService.CreateLogAsync(logText, userId);

                await transaction.CommitAsync();
                return Result.Success("صندوق با موفقیت بروزرسانی شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure($"خطا در بروزرسانی صندوق: {ex.Message}");
            }
        }

        public async Task<Result> Delete(int id, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // دریافت اطلاعات صندوق برای لاگ
                var fund = await _fundRepository.GetById(id);
                if (fund == null)
                    return Result.Failure("صندوق یافت نشد.");

                // حذف صندوق
                var dalResult = await _fundRepository.Delete(id);
                if (!dalResult.IsSuccess)
                    return Result.Failure(dalResult.Message);   // ✅ تبدیل به BLL Result

                // ذخیره تغییرات
                await _context.SaveChangesAsync();

                // ثبت لاگ
                string logText = $"حذف صندوق با نام '{fund.Name}'";
                await _logService.CreateLogAsync(logText, userId);

                await transaction.CommitAsync();
                return Result.Success("صندوق با موفقیت حذف شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure($"خطا در حذف صندوق: {ex.Message}");
            }
        }
    }
}
