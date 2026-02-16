using BusinessLogicLayer.Interface;
using DataAccessLayer;
using DataAccessLayer.Interface;
using BusinessEntity.Product;

namespace BusinessLogicLayer.Repository.Product
{
    public class TypeProdudtService : Interface.Producr.ITypeProductService
    {
        private readonly IRepository<Type_Product> _typeproductRepo;
        private readonly IRepository<BusinessEntity.Product.Product> _productRepo;
        private readonly IGenericService<Type_Product> _genericService;

        public TypeProdudtService(
            IRepository<Type_Product> typeproductRepo,
            IRepository<BusinessEntity.Product.Product> productRepo,
            IGenericService<Type_Product> genericService)
        {
            _typeproductRepo = typeproductRepo;
            _productRepo = productRepo;
            _genericService = genericService;
        }

        public async Task<IEnumerable<Type_Product>> GetAll()
        {
            return await _typeproductRepo.GetAllAsync();
        }

        public async Task<Type_Product?> GetById(int id)
        {
            return await _typeproductRepo.GetByIdAsync(id);
        }

        public async Task<Result> Create(Type_Product async, int userId)
        {
            if (string.IsNullOrWhiteSpace(async.Name))
                return Result.Failure("نام نوع کالا نمی‌تواند خالی باشد.");

            var exists = await _typeproductRepo.FindAsync(b => b.Name == async.Name);
            if (exists.Any())
                return Result.Failure("این نام قبلاً ثبت شده است.");

            string log = $"ثبت نوع کالا با نام {async.Name}";
            return await _genericService.AddWithLogAsync(async, log, userId);
        }

        public async Task<Result> Update(Type_Product entity, int userId)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
                return Result.Failure("نام نوع کالا  نمی‌تواند خالی باشد.");

            var existing = await _typeproductRepo.GetByIdAsync(entity.Id);
            if (existing == null)
                return Result.Failure("نوع کالا یافت نشد.");

            // بررسی تکراری بودن نام (به جز خودش)
            var duplicate = (await _typeproductRepo
                .FindAsync(b => b.Name == entity.Name && b.Id != entity.Id))
                .Any();

            if (duplicate)
                return Result.Failure("این نام نوع کالا قبلاً ثبت شده است.");

            // ذخیره نام قدیمی برای لاگ
            string oldName = existing.Name;

            // به‌روزرسانی خواص مجاز (در اینجا فقط Name، در صورت نیاز سایر خواص را نیز اضافه کنید)
            existing.Name = entity.Name;

            // ساختن متن لاگ
            string log = $"ویرایش نوع کالا از '{oldName}' به '{entity.Name}'";

            // استفاده از نمونه ردیابی‌شده (existing) به جای entity
            return await _genericService.UpdateWithLogAsync(existing, log, userId);
        }
        public async Task<Result> Delete(int bankId, int userId)
        {
            var bank = await _typeproductRepo.GetByIdAsync(bankId);
            if (bank == null)
                return Result.Failure("نوع کالا یافت نشد.");

            var hasAccount = (await _productRepo
                .FindAsync(a => a.Id == bankId))
                .Any();

            if (hasAccount)
                return Result.Failure("این نوع کالا دارای کالای فعال است و قابل حذف نیست.");

            string log = $"حذف نوع کالا با نام {bank.Name}";
            return await _genericService.DeleteWithLogAsync(bank, log, userId);
        }
    }
}
