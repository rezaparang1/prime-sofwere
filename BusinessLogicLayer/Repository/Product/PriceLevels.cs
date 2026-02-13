using BusinessEntity.Product;
using BusinessLogicLayer.Interface;
using DataAccessLayer;
using DataAccessLayer.Interface;

namespace BusinessLogicLayer.Repository.Product
{
    public class PricelevelService : Interface.Producr.IPriceLevelsService
    {
        private readonly IGenericRepository<PriceLevels> _pricelevelRepo;
        private readonly IGenericRepository<BusinessEntity.People.People> _peopelRepo;
        private readonly IGenericService<PriceLevels> _genericService;

        public PricelevelService(
            IGenericRepository<PriceLevels> pricelevelRepo,
            IGenericRepository<BusinessEntity.People.People> peopleRepo,
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

        public async Task<Result> Update(PriceLevels async, int userId)
        {
            if (string.IsNullOrWhiteSpace(async.Name))
                return Result.Failure("نام سطح قیمت نمی‌تواند خالی باشد.");

            var existing = await _pricelevelRepo.GetByIdAsync(async.Id);
            if (existing == null)
                return Result.Failure(" سطح قیمت یافت نشد.");

            var duplicate = (await _pricelevelRepo
                .FindAsync(b => b.Name == async.Name && b.Id != async.Id))
                .Any();

            if (duplicate)
                return Result.Failure("این نام قبلاً ثبت شده است.");

            string log = $"ویرایش سطح قیمت از '{existing.Name}' به '{async.Name}'";
            return await _genericService.UpdateWithLogAsync(async, log, userId);
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
