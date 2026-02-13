using BusinessEntity.DTO.Bank;
using BusinessLogicLayer.Interface;
using BusinessLogicLayer.Interface.Fund;
using DataAccessLayer;
using DataAccessLayer.Interface.Fund_and_Bank;
using BusinessEntity.Fund;

namespace BusinessLogicLayer.Repository.Fund
{
    public class DefinitionBankAccountService : IDefinitionBankAccountService
    {
        private readonly IDefinitionBankAccountRepository _repository;
        private readonly ILogService _logService;
        private readonly Database _context;

        public DefinitionBankAccountService(
            IDefinitionBankAccountRepository repository,
            ILogService logService,
            Database context)
        {
            _repository = repository;
            _logService = logService;
            _context = context;
        }

        public async Task<IEnumerable<BankDetailedStatementDto>> GetBankStatement(
            int? bankId = null, DateTime? dateFrom = null, DateTime? dateTo = null,
            string? receiptNumber = null, string? description = null)
        {
            return await _repository.GetBankStatement(bankId, dateFrom, dateTo,
                receiptNumber, description);
        }

        public async Task<List<Definition_Bank_Account>> Search(
            string? accountNumber = null, string? branchName = null,
            string? branchAddres = null, string? typeAccount = null,
            string? cardNumber = null, string? branchId = null,
            string? bracnhPhone = null, int? bankId = null)
        {
            return await _repository.Search(accountNumber, branchName, branchAddres,
                typeAccount, cardNumber, branchId, bracnhPhone, bankId);
        }

        public async Task<IEnumerable<Definition_Bank_Account>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Definition_Bank_Account?> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<Result> Create(Definition_Bank_Account bankAccount, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // اعتبارسنجی
                if (string.IsNullOrWhiteSpace(bankAccount.AccountNumber))
                    return Result.Failure("شماره حساب نمی‌تواند خالی باشد.");

                if (bankAccount.BankId <= 0)
                    return Result.Failure("بانک باید انتخاب شود.");

                // ایجاد حساب بانکی
                var dalResult = await _repository.Create(bankAccount);
                if (!dalResult.IsSuccess)
                    return Result.Failure(dalResult.Message);   // ✅ تبدیل به BLL Result

                // ذخیره تغییرات
                await _context.SaveChangesAsync();

                // ثبت لاگ
                string logText = $"ثبت حساب بانکی جدید با شماره حساب '{bankAccount.AccountNumber}'" +
                    $" در بانک {bankAccount.Bank?.Name}";
                await _logService.CreateLogAsync(logText, userId);

                await transaction.CommitAsync();
                return Result.Success("حساب بانکی با موفقیت ایجاد شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure($"خطا در ایجاد حساب بانکی: {ex.Message}");
            }
        }

        public async Task<Result> Update(Definition_Bank_Account bankAccount, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // اعتبارسنجی
                if (string.IsNullOrWhiteSpace(bankAccount.AccountNumber))
                    return Result.Failure("شماره حساب نمی‌تواند خالی باشد.");

                // بروزرسانی حساب بانکی
                var dalResult = await _repository.Update(bankAccount);
                if (!dalResult.IsSuccess)
                    return Result.Failure(dalResult.Message);   // ✅ تبدیل به BLL Result

                // ذخیره تغییرات
                await _context.SaveChangesAsync();

                // ثبت لاگ
                string logText = $"ویرایش حساب بانکی با شناسه {bankAccount.Id} " +
                    $"و شماره حساب '{bankAccount.AccountNumber}'";
                await _logService.CreateLogAsync(logText, userId);

                await transaction.CommitAsync();
                return Result.Success("حساب بانکی با موفقیت بروزرسانی شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure($"خطا در بروزرسانی حساب بانکی: {ex.Message}");
            }
        }

        public async Task<Result> Delete(int id, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // دریافت اطلاعات حساب بانکی برای لاگ
                var bankAccount = await _repository.GetById(id);
                if (bankAccount == null)
                    return Result.Failure("حساب بانکی یافت نشد.");

                // حذف حساب بانکی
                var dalResult = await _repository.Delete(id);
                if (!dalResult.IsSuccess)
                    return Result.Failure(dalResult.Message);   // ✅ تبدیل به BLL Result

                // ذخیره تغییرات
                await _context.SaveChangesAsync();

                // ثبت لاگ
                string logText = $"حذف حساب بانکی با شماره حساب '{bankAccount.AccountNumber}'";
                await _logService.CreateLogAsync(logText, userId);

                await transaction.CommitAsync();
                return Result.Success("حساب بانکی با موفقیت حذف شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure($"خطا در حذف حساب بانکی: {ex.Message}");
            }
        }
    }
}
