using BusinessLogicLayer.Interface;
using DataAccessLayer;
using DataAccessLayer.Interface;
using BusinessEntity.Product;

namespace BusinessLogicLayer.Repository.Product
{
    public class UnitProdudtService : BusinessLogicLayer.Interface.Producr.IUnitProductService
    {
        private readonly IRepository<Unit_Product> _unitproductRepo;
        private readonly IRepository<BusinessEntity.Product.Product> _productRepo;
        private readonly IGenericService<Unit_Product> _genericService;

        public UnitProdudtService(
            IRepository<Unit_Product> unitproductRepo,
            IRepository<BusinessEntity.Product.Product> productRepo,
            IGenericService<Unit_Product> genericService)
        {
            _unitproductRepo = unitproductRepo;
            _productRepo = productRepo;
            _genericService = genericService;
        }

        public async Task<IEnumerable<Unit_Product>> GetAll()
        {
            return await _unitproductRepo.GetAllAsync();
        }

        public async Task<Unit_Product?> GetById(int id)
        {
            return await _unitproductRepo.GetByIdAsync(id);
        }

        public async Task<Result> Create(Unit_Product async, int userId)
        {
            if (string.IsNullOrWhiteSpace(async.Name))
                return Result.Failure("نام واحد کالا نمی‌تواند خالی باشد.");

            var exists = await _unitproductRepo.FindAsync(b => b.Name == async.Name);
            if (exists.Any())
                return Result.Failure("این نام قبلاً ثبت شده است.");

            string log = $"ثبت واحد کالا با نام {async.Name}";
            return await _genericService.AddWithLogAsync(async, log, userId);
        }

        public async Task<Result> Update(Unit_Product entity, int userId)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
                return Result.Failure("نام واحد کالا  نمی‌تواند خالی باشد.");

            var existing = await _unitproductRepo.GetByIdAsync(entity.Id);
            if (existing == null)
                return Result.Failure("واحد کالا یافت نشد.");

            // بررسی تکراری بودن نام (به جز خودش)
            var duplicate = (await _unitproductRepo
                .FindAsync(b => b.Name == entity.Name && b.Id != entity.Id))
                .Any();

            if (duplicate)
                return Result.Failure("این نام واحد کالا قبلاً ثبت شده است.");

            // ذخیره نام قدیمی برای لاگ
            string oldName = existing.Name;

            // به‌روزرسانی خواص مجاز (در اینجا فقط Name، در صورت نیاز سایر خواص را نیز اضافه کنید)
            existing.Name = entity.Name;

            // ساختن متن لاگ
            string log = $"ویرایش واحد کالا از '{oldName}' به '{entity.Name}'";

            // استفاده از نمونه ردیابی‌شده (existing) به جای entity
            return await _genericService.UpdateWithLogAsync(existing, log, userId);
        }

        public async Task<Result> Delete(int bankId, int userId)
        {
            var bank = await _unitproductRepo.GetByIdAsync(bankId);
            if (bank == null)
                return Result.Failure("واحد کالا یافت نشد.");

            var hasAccount = (await _productRepo
                .FindAsync(a => a.Id == bankId))
                .Any();

            if (hasAccount)
                return Result.Failure("این واحد کالا دارای کالای فعال است و قابل حذف نیست.");

            string log = $"حذف واحد کالا با نام {bank.Name}";
            return await _genericService.DeleteWithLogAsync(bank, log, userId);
        }
    }
}
