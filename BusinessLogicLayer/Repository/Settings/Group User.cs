using BusinessLogicLayer.Interface;
using DataAccessLayer;
using DataAccessLayer.Interface;
using BusinessEntity.Settings;

namespace BusinessLogicLayer.Repository.Settings
{
    public class GroupUserService : BusinessLogicLayer.Interface.Settings.IGroupUserService
    {
        private readonly IGenericRepository<Group_User> _groupuserRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericService<Group_User> _genericService;

        public GroupUserService(
            IGenericRepository<Group_User> bankRepo,
            IGenericRepository<User> accountRepo,
            IGenericService<Group_User> genericService)
        {
            _groupuserRepo = bankRepo;
            _userRepo = accountRepo;
            _genericService = genericService;
        }

        public async Task<IEnumerable<Group_User>> GetAll()
        {
            return await _groupuserRepo.GetAllAsync();
        }

        public async Task<Group_User?> GetById(int id)
        {
            return await _groupuserRepo.GetByIdAsync(id);
        }

        public async Task<Result> Create(Group_User aync, int userId)
        {
            if (string.IsNullOrWhiteSpace(aync.Name))
                return Result.Failure("نام گروه کاربری نمی‌تواند خالی باشد.");

            var exists = await _groupuserRepo.FindAsync(b => b.Name == aync.Name);
            if (exists.Any())
                return Result.Failure("این نام گروه کاربری قبلاً ثبت شده است.");

            string log = $"ثبت گروه کاربری با نام {aync.Name}";
            return await _genericService.AddWithLogAsync(aync, log, userId);
        }

        public async Task<Result> Update(Group_User aync, int userId)
        {
            if (string.IsNullOrWhiteSpace(aync.Name))
                return Result.Failure("نام گروه کاربری نمی‌تواند خالی باشد.");

            var existingBank = await _groupuserRepo.GetByIdAsync(aync.Id);
            if (existingBank == null)
                return Result.Failure("گروه کاربری یافت نشد.");

            var duplicate = (await _groupuserRepo
                .FindAsync(b => b.Name == aync.Name && b.Id != aync.Id))
                .Any();

            if (duplicate)
                return Result.Failure("این نام گروه کاربری قبلاً ثبت شده است.");

            string log = $"ویرایش گروه کاربری از '{existingBank.Name}' به '{aync.Name}'";
            return await _genericService.UpdateWithLogAsync(aync, log, userId);
        }

        public async Task<Result> Delete(int ayncId, int userId)
        {
            var bank = await _groupuserRepo.GetByIdAsync(ayncId);
            if (bank == null)
                return Result.Failure("گروه کاربری یافت نشد.");

            var hasAccount = (await _userRepo
                .FindAsync(a => a.Id == ayncId))
                .Any();

            if (hasAccount)
                return Result.Failure("این گروه کاربری دارای کاربر فعال است و قابل حذف نیست.");

            string log = $"حذف گروه کاربری با نام {bank.Name}";
            return await _genericService.DeleteWithLogAsync(bank, log, userId);
        }
    }
}
