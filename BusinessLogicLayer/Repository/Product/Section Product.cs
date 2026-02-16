using BusinessLogicLayer.Interface;
using DataAccessLayer;
using DataAccessLayer.Interface;
using BusinessEntity.Product;

namespace BusinessLogicLayer.Repository.Product
{
    public class SectionProductService : Interface.Producr.ISectionProductService
    {
        private readonly IRepository<Section_Product> _sectionproductRepo;
        private readonly IRepository<BusinessEntity.Product.Product> _productRepo;
        private readonly IGenericService<Section_Product> _genericService;

        public SectionProductService(
            IRepository<Section_Product> sectionproductRepo,
            IRepository<BusinessEntity.Product.Product> productRepo,
            IGenericService<Section_Product> genericService)
        {
            _sectionproductRepo = sectionproductRepo;
            _productRepo = productRepo;
            _genericService = genericService;
        }

        public async Task<IEnumerable<Section_Product>> GetAll()
        {
            return await _sectionproductRepo.GetAllAsync();
        }

        public async Task<Section_Product?> GetById(int id)
        {
            return await _sectionproductRepo.GetByIdAsync(id);
        }

        public async Task<Result> Create(Section_Product async, int userId)
        {
            if (string.IsNullOrWhiteSpace(async.Name))
                return Result.Failure("نام بخش کالا نمی‌تواند خالی باشد.");

            var exists = await _sectionproductRepo.FindAsync(b => b.Name == async.Name);
            if (exists.Any())
                return Result.Failure("این نام قبلاً ثبت شده است.");

            string log = $"ثبت بخش کالا با نام {async.Name}";
            return await _genericService.AddWithLogAsync(async, log, userId);
        }

        public async Task<Result> Update(Section_Product entity, int userId)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
                return Result.Failure("نام بخش کالا  نمی‌تواند خالی باشد.");

            var existing = await _sectionproductRepo.GetByIdAsync(entity.Id);
            if (existing == null)
                return Result.Failure("بخش کالا یافت نشد.");

            // بررسی تکراری بودن نام (به جز خودش)
            var duplicate = (await _sectionproductRepo
                .FindAsync(b => b.Name == entity.Name && b.Id != entity.Id))
                .Any();

            if (duplicate)
                return Result.Failure("این نام بخش کالا قبلاً ثبت شده است.");

            // ذخیره نام قدیمی برای لاگ
            string oldName = existing.Name;

            // به‌روزرسانی خواص مجاز (در اینجا فقط Name، در صورت نیاز سایر خواص را نیز اضافه کنید)
            existing.Name = entity.Name;

            // ساختن متن لاگ
            string log = $"ویرایش بخش کالا از '{oldName}' به '{entity.Name}'";

            // استفاده از نمونه ردیابی‌شده (existing) به جای entity
            return await _genericService.UpdateWithLogAsync(existing, log, userId);
        }
        public async Task<Result> Delete(int bankId, int userId)
        {
            var bank = await _sectionproductRepo.GetByIdAsync(bankId);
            if (bank == null)
                return Result.Failure("بخش کالا یافت نشد.");

            var hasAccount = (await _productRepo
                .FindAsync(a => a.Id == bankId))
                .Any();

            if (hasAccount)
                return Result.Failure("این بخش کالا دارای کالای فعال است و قابل حذف نیست.");

            string log = $"حذف بخش کالا با نام {bank.Name}";
            return await _genericService.DeleteWithLogAsync(bank, log, userId);
        }
    }
}
