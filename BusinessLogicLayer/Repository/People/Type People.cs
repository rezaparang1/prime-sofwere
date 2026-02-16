using BusinessEntity.People;
using BusinessLogicLayer.Interface;
using DataAccessLayer;
using DataAccessLayer.Interface;


namespace BusinessLogicLayer.Repository.People
{
    public class TypePeopelService : Interface.People.ITypePeopleService
    {
        private readonly IRepository<Type_People> _typepeopleRepo;
        private readonly IRepository<BusinessEntity.People.People> _peopleRepo;
        private readonly IGenericService<Type_People> _genericService;

        public TypePeopelService(
            IRepository<Type_People> typepeopleRepo,
            IRepository<BusinessEntity.People.People> peopelRepo,
            IGenericService<Type_People> genericService)
        {
            _typepeopleRepo = typepeopleRepo;
            _peopleRepo = peopelRepo;
            _genericService = genericService;
        }

        public async Task<IEnumerable<Type_People>> GetAll()
        {
            return await _typepeopleRepo.GetAllAsync();
        }

        public async Task<Type_People?> GetById(int id)
        {
            return await _typepeopleRepo.GetByIdAsync(id);
        }

        public async Task<Result> Create(Type_People async, int userId)
        {
            if (string.IsNullOrWhiteSpace(async.Name))
                return Result.Failure("نام نوع شخص نمی‌تواند خالی باشد.");

            var exists = await _typepeopleRepo.FindAsync(b => b.Name == async.Name);
            if (exists.Any())
                return Result.Failure("این نام قبلاً ثبت شده است.");

            string log = $"ثبت نوع شخص با نام {async.Name}";
            return await _genericService.AddWithLogAsync(async, log, userId);
        }

        public async Task<Result> Update(Type_People entity, int userId)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
                return Result.Failure("نام نوع شخص نمی‌تواند خالی باشد.");

            var existing = await _typepeopleRepo.GetByIdAsync(entity.Id);
            if (existing == null)
                return Result.Failure("نوع شخص یافت نشد.");

            // بررسی تکراری بودن نام (به جز خودش)
            var duplicate = (await _typepeopleRepo
                .FindAsync(b => b.Name == entity.Name && b.Id != entity.Id))
                .Any();

            if (duplicate)
                return Result.Failure("این نوع شخص قبلاً ثبت شده است.");

            // ذخیره نام قدیمی برای لاگ
            string oldName = existing.Name;

            // به‌روزرسانی خواص مجاز (در اینجا فقط Name، در صورت نیاز سایر خواص را نیز اضافه کنید)
            existing.Name = entity.Name;

            // ساختن متن لاگ
            string log = $"ویرایش نوع شخص  از '{oldName}' به '{entity.Name}'";

            // استفاده از نمونه ردیابی‌شده (existing) به جای entity
            return await _genericService.UpdateWithLogAsync(existing, log, userId);
        }

        public async Task<Result> Delete(int bankId, int userId)
        {
            var bank = await _typepeopleRepo.GetByIdAsync(bankId);
            if (bank == null)
                return Result.Failure("نوع شخص یافت نشد.");

            var hasAccount = (await _peopleRepo
                .FindAsync(a => a.Id == bankId))
                .Any();

            if (hasAccount)
                return Result.Failure("این نوع شخص دارای شخص فعال است و قابل حذف نیست.");

            string log = $"حذف نوع شخص با نام {bank.Name}";
            return await _genericService.DeleteWithLogAsync(bank, log, userId);
        }
    }
}
