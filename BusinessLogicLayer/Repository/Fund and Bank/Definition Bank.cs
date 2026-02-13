using BusinessEntity.Fund;
using BusinessLogicLayer.Interface;
using DataAccessLayer;
using DataAccessLayer.Interface;

namespace BusinessLogicLayer.Repository.Fund
{
    public class DefinitionBankService : BusinessLogicLayer.Interface.Fund.IDefinitionBankService
    {
        private readonly IGenericRepository<Definition_Bank> _bankRepo;
        private readonly IGenericRepository<Definition_Bank_Account> _accountRepo;
        private readonly IGenericService<Definition_Bank> _genericService;

        public DefinitionBankService(
            IGenericRepository<Definition_Bank> bankRepo,
            IGenericRepository<Definition_Bank_Account> accountRepo,
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

        public async Task<Result> Create(Definition_Bank async, int userId)
        {
            if (string.IsNullOrWhiteSpace(async.Name))
                return Result.Failure("نام بانک نمی‌تواند خالی باشد.");

            var exists = await _bankRepo.FindAsync(b => b.Name == async.Name);
            if (exists.Any())
                return Result.Failure("این نام بانک قبلاً ثبت شده است.");

            string log = $"ثبت بانک با نام {async.Name}";
            return await _genericService.AddWithLogAsync(async, log, userId);
        }

        public async Task<Result> Update(Definition_Bank async, int userId)
        {
            if (string.IsNullOrWhiteSpace(async.Name))
                return Result.Failure("نام بانک نمی‌تواند خالی باشد.");

            var existing = await _bankRepo.GetByIdAsync(async.Id);
            if (existing == null)
                return Result.Failure("بانک یافت نشد.");

            var duplicate = (await _bankRepo
                .FindAsync(b => b.Name == async.Name && b.Id != async.Id))
                .Any();

            if (duplicate)
                return Result.Failure("این نام بانک قبلاً ثبت شده است.");

            string log = $"ویرایش بانک از '{existing.Name}' به '{async.Name}'";
            return await _genericService.UpdateWithLogAsync(async, log, userId);
        }

        public async Task<Result> Delete(int Id, int userId)
        {
            var bank = await _bankRepo.GetByIdAsync(Id);
            if (bank == null)
                return Result.Failure("بانک یافت نشد.");

            var hasAccount = (await _accountRepo
                .FindAsync(a => a.BankId == Id))
                .Any();

            if (hasAccount)
                return Result.Failure("این بانک دارای حساب فعال است و قابل حذف نیست.");

            string log = $"حذف بانک با نام {bank.Name}";
            return await _genericService.DeleteWithLogAsync(bank, log, userId);
        }
    }
}
