using BusinessEntity.DTO.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DataAccessLayer.Interface.Product;


namespace DataAccessLayer.Repository.Product
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly Database _context;
        private readonly ILogger<PeopleRepository> _logger;

        public PeopleRepository(Database context, ILogger<PeopleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ***** GetPeopleForCombo *****
        public async Task<List<PeopleComboDto>> GetPeopleForComboAsync()
        {
            return await _context.People
                .Where(p => !p.IsDelete)
                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName)
                .Select(p => new PeopleComboDto
                {
                    Id = p.Id,
                    FullName = p.FirstName + " " + p.LastName
                })
                .ToListAsync();
        }

        // ***** Search *****
        public async Task<List<BusinessEntity.People.People>> Search(
            string? firstName = null, string? lastName = null,
            string? phone = null, string? address = null, int? groupPeople = null,
            bool? business = null, bool? user = null, bool? employee = null,
            bool? investor = null)
        {
            var query = _context.People
                .Include(p => p.Group_People)
                .Include(p => p.Type_People)
                .Include(p => p.Account)
                .Where(p => !p.IsDelete);

            if (!string.IsNullOrEmpty(firstName))
                query = query.Where(r => r.FirstName.Contains(firstName));

            if (!string.IsNullOrEmpty(lastName))
                query = query.Where(r => r.LastName.Contains(lastName));

            if (!string.IsNullOrEmpty(phone))
                query = query.Where(r => r.Phone == phone);

            if (!string.IsNullOrEmpty(address))
                query = query.Where(r => r.Address.Contains(address));

            if (groupPeople.HasValue)
                query = query.Where(r => r.GroupPeopleId == groupPeople.Value);

            if (business.HasValue)
                query = query.Where(r => r.Business == business.Value);

            if (user.HasValue)
                query = query.Where(r => r.User == user.Value);

            if (employee.HasValue)
                query = query.Where(r => r.Employee == employee.Value);

            if (investor.HasValue)
                query = query.Where(r => r.Investor == investor.Value);

            return await query.OrderBy(r => r.LastName).ThenBy(r => r.FirstName).ToListAsync();
        }

        // ***** READ *****
        public async Task<BusinessEntity.People.People?> GetPeopleById(string? idPeople)
        {
            if (string.IsNullOrWhiteSpace(idPeople))
                return null;

            return await _context.People
                .Include(p => p.Account)
                .Include(p => p.Group_People)
                .Include(p => p.Type_People)
                .FirstOrDefaultAsync(r => r.IdPeople == idPeople && !r.IsDelete);
        }

        public async Task<IEnumerable<BusinessEntity.People.People>> GetAll()
        {
            return await _context.People
                .Include(p => p.Account)
                .Include(p => p.Group_People)
                .Include(p => p.Type_People)
                .Where(p => !p.IsDelete)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToListAsync();
        }

        public async Task<BusinessEntity.People.People?> GetById(int id)
        {
            return await _context.People
                .Include(p => p.Account)
                .Include(p => p.Group_People)
                .Include(p => p.Type_People)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDelete);
        }

        // ***** CREATE *****
        public async Task<Result> Create(BusinessEntity.People.People person)
        {
            try
            {
                // بررسی تکراری بودن کد شخص
                if (await _context.People
                    .AnyAsync(i => i.IdPeople == person.IdPeople && !i.IsDelete))
                    return Result.Failure("کد شخص وارد شده تکراری است.");

                // بررسی تکراری بودن شماره تماس
                if (await _context.People
                    .AnyAsync(i => i.Phone == person.Phone && !i.IsDelete))
                    return Result.Failure("شماره تماس وارد شده تکراری است.");

                // ایجاد حساب مالی مرتبط
                string accountName = $"{person.FirstName} {person.LastName}".Trim();
                bool accountExists = await _context.Account
                    .AnyAsync(a => a.AccountName == accountName && !a.IsDelete);

                if (accountExists)
                    return Result.Failure("حساب مالی با این نام قبلاً وجود دارد.");

                var account = new BusinessEntity.Invoices.Account
                {
                    AccountName = accountName,
                    AccountType = "Person",
                    Balance = person.Inventory,
                    IsDelete = false
                };

                person.Account = account;
                person.IsDelete = false;

                await _context.People.AddAsync(person);
                return Result.Success("عملیات با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Person: {@Person}", person);
                return Result.Failure($"خطا در ایجاد شخص: {ex.Message}");
            }
        }

        // ***** UPDATE *****
        public async Task<Result> Update(BusinessEntity.People.People person)
        {
            try
            {
                // بررسی وجود شخص
                var existing = await _context.People
                    .Include(p => p.Account)
                    .FirstOrDefaultAsync(p => p.Id == person.Id && !p.IsDelete);

                if (existing == null)
                    return Result.Failure("شخص یافت نشد.");

                // بررسی تکراری بودن کد شخص (به جز خودش)
                if (await _context.People
                    .AnyAsync(i => i.IdPeople == person.IdPeople && i.Id != person.Id && !i.IsDelete))
                    return Result.Failure("کد شخص وارد شده تکراری است.");

                // بررسی تکراری بودن شماره تماس (به جز خودش)
                if (await _context.People
                    .AnyAsync(i => i.Phone == person.Phone && i.Id != person.Id && !i.IsDelete))
                    return Result.Failure("شماره تماس وارد شده تکراری است.");

                // به‌روزرسانی فیلدها
                existing.IdPeople = person.IdPeople;
                existing.TypePeopleId = person.TypePeopleId;
                existing.FirstName = person.FirstName;
                existing.LastName = person.LastName;
                existing.Phone = person.Phone;
                existing.CreditLimit = person.CreditLimit;
                existing.IsCreditLimit = person.IsCreditLimit;
                existing.HowToDoBusiness = person.HowToDoBusiness;
                existing.OFF = person.OFF;
                existing.Business = person.Business;
                existing.User = person.User;
                existing.Employee = person.Employee;
                existing.Investor = person.Investor;
                existing.Description = person.Description;
                existing.Address = person.Address;
                existing.TaxFree = person.TaxFree;
                existing.InitialCapital = person.InitialCapital;
                existing.Inventory = person.Inventory;
                existing.GroupPeopleId = person.GroupPeopleId;
                existing.PriceLevelID = person.PriceLevelID;

                // به‌روزرسانی حساب مالی مرتبط
                if (existing.Account != null)
                {
                    existing.Account.AccountName = $"{person.FirstName} {person.LastName}".Trim();
                    existing.Account.Balance = person.Inventory;
                }

                _context.People.Update(existing);
                return Result.Success("عملیات با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Person: {@Person}", person);
                return Result.Failure($"خطا در بروزرسانی شخص: {ex.Message}");
            }
        }

        // ***** DELETE *****
        public async Task<Result> Delete(int id)
        {
            try
            {
                var person = await _context.People
                    .Include(p => p.Users)
                    .Include(p => p.Account)
                    .FirstOrDefaultAsync(p => p.Id == id && !p.IsDelete);

                if (person == null)
                    return Result.Failure("شخص یافت نشد.");

                // بررسی وجود کاربر فعال برای این شخص
                if (person.Users.Any(u => !u.IsDelete))
                    return Result.Failure("امکان حذف شخص وجود ندارد، زیرا کاربر فعال برای آن ثبت شده است.");

                // بررسی وجود تراکنش مالی برای حساب این شخص
                bool hasTransaction = await _context.Transaction
                    .AnyAsync(t => t.AccountId == person.AccountId);

                if (hasTransaction)
                    return Result.Failure("امکان حذف شخص وجود ندارد، زیرا تراکنش‌های مالی برای آن ثبت شده است.");

                // Soft Delete
                person.IsDelete = true;
                if (person.Account != null)
                    person.Account.IsDelete = true;

                return Result.Success("عملیات با موفقیت انجام شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Person with ID: {Id}", id);
                return Result.Failure($"خطا در حذف شخص: {ex.Message}");
            }
        }
    }
}
