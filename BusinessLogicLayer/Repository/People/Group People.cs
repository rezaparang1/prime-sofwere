using BusinessLogicLayer.Interface;
using DataAccessLayer.Interface;
using DataAccessLayer.Interface;
using BusinessEntity.People;

namespace BusinessLogicLayer.Repository.People
{
    public class GroupPeopelService : Interface.People.IGroupPeopleService
    {
        private readonly IRepository<Group_People> _grouppeopleRepo;
        private readonly IRepository<BusinessEntity.People.People> _peopleRepo;
        private readonly IGenericService<Group_People> _genericService;

        public GroupPeopelService(
            IRepository<Group_People> grouppeopleRepo,
            IRepository<BusinessEntity.People.People> peopeleRepo,
            IGenericService<Group_People> genericService)
        {
            _grouppeopleRepo = grouppeopleRepo;
            _peopleRepo = peopeleRepo;
            _genericService = genericService;
        }

        public async Task<IEnumerable<Group_People>> GetAll()
        {
            return await _grouppeopleRepo.GetAllAsync();
        }

        public async Task<Group_People?> GetById(int id)
        {
            return await _grouppeopleRepo.GetByIdAsync(id);
        }

        public async Task<Result> Create(Group_People async, int userId)
        {
            if (string.IsNullOrWhiteSpace(async.Name))
                return Result.Failure("نام گروه شخص نمی‌تواند خالی باشد.");

            var exists = await _grouppeopleRepo.FindAsync(b => b.Name == async.Name);
            if (exists.Any())
                return Result.Failure("این نام قبلاً ثبت شده است.");

            string log = $"ثبت گروه شخص با نام {async.Name}";
            return await _genericService.AddWithLogAsync(async, log, userId);
        }

        public async Task<Result> Update(Group_People entity, int userId)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
                return Result.Failure("نام گروه شخص نمی‌تواند خالی باشد.");

            var existing = await _grouppeopleRepo.GetByIdAsync(entity.Id);
            if (existing == null)
                return Result.Failure("گروه شخص یافت نشد.");

            // بررسی تکراری بودن نام (به جز خودش)
            var duplicate = (await _grouppeopleRepo
                .FindAsync(b => b.Name == entity.Name && b.Id != entity.Id))
                .Any();

            if (duplicate)
                return Result.Failure("این گروه شخص قبلاً ثبت شده است.");

            // ذخیره نام قدیمی برای لاگ
            string oldName = existing.Name;

            // به‌روزرسانی خواص مجاز (در اینجا فقط Name، در صورت نیاز سایر خواص را نیز اضافه کنید)
            existing.Name = entity.Name;

            // ساختن متن لاگ
            string log = $"ویرایش گروه شخص  از '{oldName}' به '{entity.Name}'";

            // استفاده از نمونه ردیابی‌شده (existing) به جای entity
            return await _genericService.UpdateWithLogAsync(existing, log, userId);
        }


        public async Task<Result> Delete(int bankId, int userId)
        {
            var bank = await _grouppeopleRepo.GetByIdAsync(bankId);
            if (bank == null)
                return Result.Failure("گروه شخص یافت نشد.");

            var hasAccount = (await _peopleRepo
                .FindAsync(a => a.Id == bankId))
                .Any();

            if (hasAccount)
                return Result.Failure("این گروه شخص دارای شخص فعال است و قابل حذف نیست.");

            string log = $"حذف گروه شخص با نام {bank.Name}";
            return await _genericService.DeleteWithLogAsync(bank, log, userId);
        }
    }
}
