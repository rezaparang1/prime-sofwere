using BusinessLogicLayer.Interface;
using DataAccessLayer;
using DataAccessLayer.Interface;
using BusinessEntity.Product;

namespace BusinessLogicLayer.Repository.Product
{
    public class GroupProductService : BusinessLogicLayer.Interface.Product.IGroupProductService
    {
        private readonly IRepository<Group_Product> _groupoductRepo;
        private readonly IRepository<BusinessEntity.Product.Product> _productRepo;
        private readonly IGenericService<Group_Product> _genericService;

        public GroupProductService(
            IRepository<Group_Product> groupproductRepo,
            IRepository<BusinessEntity.Product.Product> productRepo,
            IGenericService<Group_Product> genericService)
        {
            _groupoductRepo = groupproductRepo;
            _productRepo = productRepo;
            _genericService = genericService;
        }

        public async Task<IEnumerable<Group_Product>> GetAll()
        {
            return await _groupoductRepo.GetAllAsync();
        }

        public async Task<Group_Product?> GetById(int id)
        {
            return await _groupoductRepo.GetByIdAsync(id);
        }

        public async Task<Result> Create(Group_Product async, int UserId)
        {
            if (string.IsNullOrWhiteSpace(async.Name))
                return Result.Failure("نام گروه کالا نمی‌تواند خالی باشد.");

            var exists = await _groupoductRepo.FindAsync(b => b.Name == async.Name);
            if (exists.Any())
                return Result.Failure("این نام قبلاً ثبت شده است.");

            string log = $"ثبت گروه کالا با نام {async.Name}";
            return await _genericService.AddWithLogAsync(async, log, UserId);
        }

        public async Task<Result> Update(Group_Product async, int UserId)
        {
            if (string.IsNullOrWhiteSpace(async.Name))
                return Result.Failure("نام گروه کالا نمی‌تواند خالی باشد.");

            var existing = await _groupoductRepo.GetByIdAsync(async.Id);
            if (existing == null)
                return Result.Failure("گروه کالا یافت نشد.");

            var duplicate = (await _groupoductRepo
                .FindAsync(b => b.Name == async.Name && b.Id != async.Id))
                .Any();

            if (duplicate)
                return Result.Failure("این نام قبلاً ثبت شده است.");

            string log = $"ویرایش گروه کالا از '{existing.Name}' به '{async.Name}'";
            return await _genericService.UpdateWithLogAsync(async, log, UserId);
        }

        public async Task<Result> Delete(int Id, int UserId)
        {
            var bank = await _groupoductRepo.GetByIdAsync(Id);
            if (bank == null)
                return Result.Failure("گروه کالا یافت نشد.");

            var hasAccount = (await _productRepo
                .FindAsync(a => a.Id == Id))
                .Any();

            if (hasAccount)
                return Result.Failure("این گروه کالا دارای کالای فعال است و قابل حذف نیست.");

            string log = $"حذف گروه کالا با نام {bank.Name}";
            return await _genericService.DeleteWithLogAsync(bank, log, UserId);
        }
    }
}
