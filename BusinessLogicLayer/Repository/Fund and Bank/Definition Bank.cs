using BusinessEntity.Fund;
using BusinessLogicLayer.Interface;
using BusinessLogicLayer.Interface.Fund;
using DataAccessLayer;
using DataAccessLayer.Interface;

namespace BusinessLogicLayer.Repository.Fund
{
    public class DefinitionBankService : IDefinitionBankService
    {
        private readonly IRepository<Definition_Bank> _bankRepo;
        private readonly IRepository<Definition_Bank_Account> _accountRepo;
        private readonly IGenericService<Definition_Bank> _genericService;

        public DefinitionBankService(
            IRepository<Definition_Bank> bankRepo,
            IRepository<Definition_Bank_Account> accountRepo,
            IGenericService<Definition_Bank> genericService)
        {
            _bankRepo = bankRepo;
            _accountRepo = accountRepo;
            _genericService = genericService;
        }

        public async Task<IEnumerable<Definition_Bank>> GetAll()
        {
            return await _bankRepo.GetAllAsync();
        }

        public async Task<Definition_Bank?> GetById(int id)
        {
            return await _bankRepo.GetByIdAsync(id);
        }

        public async Task<Result> Create(Definition_Bank entity, int userId)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
                return Result.Failure("نام بانک نمی‌تواند خالی باشد.");

            var exists = await _bankRepo.FindAsync(b => b.Name == entity.Name);
            if (exists.Any())
                return Result.Failure("این نام بانک قبلاً ثبت شده است.");

            string log = $"ثبت بانک با نام {entity.Name}";
            return await _genericService.AddWithLogAsync(entity, log, userId);
        }
        public async Task<Result> Update(Definition_Bank entity, int userId)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
                return Result.Failure("نام بانک نمی‌تواند خالی باشد.");

            var existing = await _bankRepo.GetByIdAsync(entity.Id);
            if (existing == null)
                return Result.Failure("بانک یافت نشد.");

            // بررسی تکراری بودن نام (به جز خودش)
            var duplicate = (await _bankRepo
                .FindAsync(b => b.Name == entity.Name && b.Id != entity.Id))
                .Any();

            if (duplicate)
                return Result.Failure("این نام بانک قبلاً ثبت شده است.");

            // ذخیره نام قدیمی برای لاگ
            string oldName = existing.Name;

            // به‌روزرسانی خواص مجاز (در اینجا فقط Name، در صورت نیاز سایر خواص را نیز اضافه کنید)
            existing.Name = entity.Name;

            // ساختن متن لاگ
            string log = $"ویرایش بانک از '{oldName}' به '{entity.Name}'";

            // استفاده از نمونه ردیابی‌شده (existing) به جای entity
            return await _genericService.UpdateWithLogAsync(existing, log, userId);
        }
        public async Task<Result> Delete(int id, int userId)
        {
            var bank = await _bankRepo.GetByIdAsync(id);
            if (bank == null)
                return Result.Failure("بانک یافت نشد.");

            var hasAccount = (await _accountRepo
                .FindAsync(a => a.BankId == id))
                .Any();

            if (hasAccount)
                return Result.Failure("این بانک دارای حساب فعال است و قابل حذف نیست.");

            string log = $"حذف بانک با نام {bank.Name}";
            return await _genericService.DeleteWithLogAsync(bank, log, userId);
        }
    }
}
