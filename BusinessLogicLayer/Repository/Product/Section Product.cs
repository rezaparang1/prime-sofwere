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

        public async Task<Result> Update(Section_Product async, int userId)
        {
            if (string.IsNullOrWhiteSpace(async.Name))
                return Result.Failure("نام بخش کالا نمی‌تواند خالی باشد.");

            var existing = await _sectionproductRepo.GetByIdAsync(async.Id);
            if (existing == null)
                return Result.Failure("بخش کالا یافت نشد.");

            var duplicate = (await _sectionproductRepo
                .FindAsync(b => b.Name == async.Name && b.Id != async.Id))
                .Any();

            if (duplicate)
                return Result.Failure("این نام قبلاً ثبت شده است.");

            string log = $"ویرایش بخش کالا از '{existing.Name}' به '{async.Name}'";
            return await _genericService.UpdateWithLogAsync(async, log, userId);
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
