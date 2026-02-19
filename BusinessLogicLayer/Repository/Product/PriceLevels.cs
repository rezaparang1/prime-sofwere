using BusinessEntity.Product;
using BusinessLogicLayer.Interface;
using DataAccessLayer;
using DataAccessLayer.Interface;

namespace BusinessLogicLayer.Repository.Product
{
    public class PricelevelService : BusinessLogicLayer.Interface.Product.IPriceLevelsService
    {
        private readonly IRepository<PriceLevels> _pricelevelRepo;
        private readonly IRepository<BusinessEntity.People.People> _peopelRepo;
        private readonly IGenericService<PriceLevels> _genericService;

        public PricelevelService(
            IRepository<PriceLevels> pricelevelRepo,
            IRepository<BusinessEntity.People.People> peopleRepo,
            IGenericService<PriceLevels> genericService)
        {
            _pricelevelRepo = pricelevelRepo;
            _peopelRepo = peopleRepo;
            _genericService = genericService;
        }

        public async Task<IEnumerable<PriceLevels>> GetAll()
        {
            return await _pricelevelRepo.GetAllAsync();
        }

        public async Task<PriceLevels?> GetById(int id)
        {
            return await _pricelevelRepo.GetByIdAsync(id);
        }

        public async Task<Result> Create(PriceLevels async, int userId)
        {
            if (string.IsNullOrWhiteSpace(async.Name))
                return Result.Failure("نام سطح قیمت نمی‌تواند خالی باشد.");

            var exists = await _pricelevelRepo.FindAsync(b => b.Name == async.Name);
            if (exists.Any())
                return Result.Failure("این نام قبلاً ثبت شده است.");

            string log = $"ثبت  سطح قیمت با نام {async.Name}";
            return await _genericService.AddWithLogAsync(async, log, userId);
        }

        public async Task<Result> Update(PriceLevels entity, int userId)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
                return Result.Failure("نام سطح قیمت نمی‌تواند خالی باشد.");

            var existing = await _pricelevelRepo.GetByIdAsync(entity.Id);
            if (existing == null)
                return Result.Failure("سطح قیمت  یافت نشد.");

            // بررسی تکراری بودن نام (به جز خودش)
            var duplicate = (await _pricelevelRepo
                .FindAsync(b => b.Name == entity.Name && b.Id != entity.Id))
                .Any();

            if (duplicate)
                return Result.Failure("این نام سطح قیمت قبلاً ثبت شده است.");

            // ذخیره نام قدیمی برای لاگ
            string oldName = existing.Name;

            // به‌روزرسانی خواص مجاز (در اینجا فقط Name، در صورت نیاز سایر خواص را نیز اضافه کنید)
            existing.Name = entity.Name;

            // ساختن متن لاگ
            string log = $"ویرایش سطح قیمت از '{oldName}' به '{entity.Name}'";

            // استفاده از نمونه ردیابی‌شده (existing) به جای entity
            return await _genericService.UpdateWithLogAsync(existing, log, userId);
        }
        public async Task<Result> Delete(int bankId, int userId)
        {
            var bank = await _pricelevelRepo.GetByIdAsync(bankId);
            if (bank == null)
                return Result.Failure(" سطح قیمت یافت نشد.");

            var hasAccount = (await _peopelRepo
                .FindAsync(a => a.Id == bankId))
                .Any();

            if (hasAccount)
                return Result.Failure("این سطح قیمت دارای شخص فعال است و قابل حذف نیست.");

            string log = $"حذف  سطح قیمت با نام {bank.Name}";
            return await _genericService.DeleteWithLogAsync(bank, log, userId);
        }
    }
}
