using BusinessEntity.DTO.People;
using BusinessLogicLayer.Interface;
using BusinessLogicLayer.Interface.People;
using DataAccessLayer;
using DataAccessLayer.Interface.Product;

namespace BusinessLogicLayer.Repository.People
{
    public class PeopleService : IPeopleService
    {
        private readonly IPeopleRepository _peopleRepository;
        private readonly ILogService _logService;
        private readonly Database _context;

        public PeopleService(
            IPeopleRepository peopleRepository,
            ILogService logService,
            Database context)
        {
            _peopleRepository = peopleRepository;
            _logService = logService;
            _context = context;
        }

        public async Task<List<PeopleComboDto>> GetPeopleForComboAsync()
        {
            return await _peopleRepository.GetPeopleForComboAsync();
        }

        public async Task<List<BusinessEntity.People.People>> Search(
            string? firstName = null, string? lastName = null,
            string? phone = null, string? address = null, int? groupPeople = null,
            bool? business = null, bool? user = null, bool? employee = null,
            bool? investor = null)
        {
            return await _peopleRepository.Search(firstName, lastName, phone, address,
                groupPeople, business, user, employee, investor);
        }

        public async Task<IEnumerable<BusinessEntity.People.People>> GetAll()
        {
            return await _peopleRepository.GetAll();
        }

        public async Task<BusinessEntity.People.People?> GetById(int id)
        {
            return await _peopleRepository.GetById(id);
        }

        public async Task<BusinessEntity.People.People?> GetPeopleById(string? idPeople)
        {
            return await _peopleRepository.GetPeopleById(idPeople);
        }

        public async Task<Result> Create(BusinessEntity.People.People person, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // اعتبارسنجی
                if (string.IsNullOrWhiteSpace(person.IdPeople))
                    return Result.Failure("کد شخص نمی‌تواند خالی باشد.");

                if (string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName))
                    return Result.Failure("نام و نام خانوادگی نمی‌تواند خالی باشد.");

                if (string.IsNullOrWhiteSpace(person.Phone))
                    return Result.Failure("شماره تماس نمی‌تواند خالی باشد.");

                if (person.TypePeopleId <= 0)
                    return Result.Failure("نوع شخص باید انتخاب شود.");

                // ایجاد شخص
                var result = await _peopleRepository.Create(person);
                if (!result.IsSuccess)
                    return result;

                // ذخیره تغییرات
                await _context.SaveChangesAsync();

                // ثبت لاگ
                string logText = $"ثبت شخص جدید با نام '{person.FirstName} {person.LastName}' و کد '{person.IdPeople}'";
                await _logService.CreateLogAsync(logText, userId);

                await transaction.CommitAsync();
                return Result.Success("شخص با موفقیت ایجاد شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure($"خطا در ایجاد شخص: {ex.Message}");
            }
        }

        public async Task<Result> Update(BusinessEntity.People.People person, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // اعتبارسنجی
                if (string.IsNullOrWhiteSpace(person.IdPeople))
                    return Result.Failure("کد شخص نمی‌تواند خالی باشد.");

                if (string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName))
                    return Result.Failure("نام و نام خانوادگی نمی‌تواند خالی باشد.");

                // بروزرسانی شخص
                var result = await _peopleRepository.Update(person);
                if (!result.IsSuccess)
                    return result;

                // ذخیره تغییرات
                await _context.SaveChangesAsync();

                // ثبت لاگ
                string logText = $"ویرایش شخص با شناسه {person.Id} و نام '{person.FirstName} {person.LastName}'";
                await _logService.CreateLogAsync(logText, userId);

                await transaction.CommitAsync();
                return Result.Success("شخص با موفقیت بروزرسانی شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure($"خطا در بروزرسانی شخص: {ex.Message}");
            }
        }

        public async Task<Result> Delete(int id, int userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // دریافت اطلاعات شخص برای لاگ
                var person = await _peopleRepository.GetById(id);
                if (person == null)
                    return Result.Failure("شخص یافت نشد.");

                // حذف شخص
                var result = await _peopleRepository.Delete(id);
                if (!result.IsSuccess)
                    return result;

                // ذخیره تغییرات
                await _context.SaveChangesAsync();

                // ثبت لاگ
                string logText = $"حذف شخص با نام '{person.FirstName} {person.LastName}'";
                await _logService.CreateLogAsync(logText, userId);

                await transaction.CommitAsync();
                return Result.Success("شخص با موفقیت حذف شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result.Failure($"خطا در حذف شخص: {ex.Message}");
            }
        }
    }
}
