using BusinessEntity.People;
using BusinessEntity.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.People
{
    public class PeopleRepository : Interface.People.IPeopleRepository
    {
        private readonly Database _context;
        private readonly ILogger<PeopleRepository> _logger;

        public PeopleRepository(Database context, ILogger<PeopleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        //*****SEARCH*****
        public async Task<List<BusinessEntity.People.People>> Search(
            string? firstName = null, string? lastName = null, string? peoplId = null,
            string? phone = null, string? address = null, int? gropPeople = null,
            bool? business = null, bool? user = null, bool? employee = null, bool? investor = null)
        {
            _logger.LogInformation("در حال جستجوی اشخاص با فیلترها...");

            var query = _context.People
                .Include(p => p.Group_People)
                .Include(p => p.Type_People)
                .AsQueryable();

            if (!string.IsNullOrEmpty(firstName))
                query = query.Where(r => r.FirstName.Contains(firstName));

            if (!string.IsNullOrEmpty(lastName))
                query = query.Where(r => r.LastName.Contains(lastName));

            if (!string.IsNullOrEmpty(peoplId))
                query = query.Where(r => r.IdPeople == peoplId);

            if (!string.IsNullOrEmpty(phone))
                query = query.Where(r => r.Phone.Contains(phone));

            if (!string.IsNullOrEmpty(address))
                query = query.Where(r => r.Address.Contains(address));

            if (gropPeople.HasValue)
                query = query.Where(r => r.GroupPeopleId == gropPeople.Value);

            if (business.HasValue)
                query = query.Where(r => r.Business == business.Value);

            if (user.HasValue)
                query = query.Where(r => r.User == user.Value);

            if (employee.HasValue)
                query = query.Where(r => r.Employee == employee.Value);

            if (investor.HasValue)
                query = query.Where(r => r.Investor == investor.Value);

            var result = await query.OrderBy(r => r.LastName).ThenBy(r => r.FirstName).ToListAsync();
            _logger.LogInformation("{Count} شخص یافت شد.", result.Count);
            return result;
        }

        //******READ*******
        public async Task<List<PeopleComboDto>> GetPeopleForComboAsync()
        {
            return await _context.People
                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName)
                .Select(p => new PeopleComboDto
                {
                    Id = p.Id,
                    FullName = p.FirstName + " " + p.LastName
                })
                .ToListAsync();
        }
        public async Task<BusinessEntity.People.People?> GetPeolpeById(string? IdPeople)
        {
            _logger.LogInformation("در حال جستجو برای شناسه شخص: {IdPeople}", IdPeople);

            if (string.IsNullOrWhiteSpace(IdPeople))
            {
                _logger.LogWarning("پارامتر IdPeople مقدار ندارد.");
                return null;
            }

            try
            {
                var result = await _context.People
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.IdPeople == IdPeople);

                if (result == null)
                {
                    _logger.LogWarning("هیچ شخصی با شناسه {IdPeople} یافت نشد.", IdPeople);
                }
                else
                {
                    _logger.LogInformation("شخص با شناسه {IdPeople} با موفقیت پیدا شد.", IdPeople);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در هنگام جستجوی شخص با شناسه {IdPeople}", IdPeople);
                return null;
            }
        }
        public async Task<IEnumerable<BusinessEntity.People.People>> GetAll()
        {
            var result = await _context.People
                .Include(p => p.Account)
                .Include(p => p.Group_People)
                .OrderBy(p => p.LastName)
                .ToListAsync();

            _logger.LogInformation("{Count} رکورد شخص بازیابی شد.", result.Count);
            return result;
        }
        public async Task<BusinessEntity.People.People?> GetById(int id)
        {
            var entity = await _context.People
                .Include(p => p.Account)
                .Include(p => p.Group_People)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
                _logger.LogWarning("شخص با شناسه {Id} یافت نشد.", id);
            else
                _logger.LogInformation("شخص {Id} با موفقیت بازیابی شد.", id);

            return entity;
        }
        //******CRUD*******
        public async Task<string> Create(int userId, BusinessEntity.People.People person)
        {
            if (person == null)
                return "داده‌ای ارسال نشده است.";

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("در حال ایجاد شخص جدید: {@Person}", person);

                if (await _context.People.AnyAsync(i => i.IdPeople == person.IdPeople))
                    return "کد شخص وارد شده تکراری است.";

                if (await _context.People.AnyAsync(i => i.Phone == person.Phone))
                    return "شماره تماس وارد شده تکراری است.";

                // 🧾 ایجاد حساب مالی مرتبط
                string accountName = $"{person.FirstName} {person.LastName}".Trim();
                bool accountExists = await _context.Account.AnyAsync(a => a.AccountName == accountName);
                if (accountExists)
                    return "حساب مالی با این نام قبلاً وجود دارد.";

                var account = new BusinessEntity.Financial_Operations.Account
                {
                    AccountName = accountName,
                    AccountType = "Person",
                    Balance = person.Inventory
                };

                person.Account = account;

                await _context.People.AddAsync(person);
                await _context.SaveChangesAsync();

                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"ثبت شخص جدید ({person.FirstName} {person.LastName}) و ایجاد حساب مالی مرتبط",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در ایجاد شخص جدید: {@Person}", person);
                return $"خطایی در ذخیره اطلاعات رخ داد: {ex.Message}";
            }
        }
        public async Task<string> Update(int userId, BusinessEntity.People.People person)
        {
            try
            {
                if (person == null)
                    throw new ArgumentNullException(nameof(person));

                if (await _context.People.AnyAsync(i => i.IdPeople == person.IdPeople && i.Id != person.Id))
                    return "کد شخص وارد شده تکراری است.";

                if (await _context.People.AnyAsync(i => i.Phone == person.Phone && i.Id != person.Id))
                    return "شماره تماس وارد شده تکراری است.";

                var existing = await _context.People
                    .Include(p => p.Account)
                    .FirstOrDefaultAsync(p => p.Id == person.Id);

                if (existing == null)
                    return "شناسه وارد شده با رکورد موجود مطابقت ندارد.";

                _context.Entry(existing).CurrentValues.SetValues(person);

                if (existing.Account != null)
                {
                    existing.Account.AccountName = $"{person.FirstName} {person.LastName}".Trim();
                    existing.Account.Balance = person.Inventory;
                }

                await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
                {
                    Description = $"ویرایش اطلاعات شخص ({person.FirstName} {person.LastName})",
                    UserId = userId,
                    Date = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                return "عملیات با موفقیت انجام شد.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ویرایش شخص: {@Person}", person);
                return $"خطایی در ذخیره تغییرات رخ داد: {ex.Message}";
            }
        }
        public async Task<string> Delete(int userId, int id)
        {
            var entity = await _context.People
                .Include(p => p.Users)
                .Include(p => p.Account)
                .IgnoreQueryFilters() // در صورتی که قبلاً حذف شده
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
                return "رکورد مورد نظر یافت نشد.";

            if (entity.IsDelete)
                return "این شخص قبلاً غیرفعال شده است.";

            // چک User وابسته
            if (entity.Users.Any(u => !u.IsDelete))
                return "امکان حذف شخص وجود ندارد، زیرا کاربر فعال برای آن ثبت شده است.";

            // چک تراکنش‌های مالی
            bool hasTransaction = await _context.Transaction
                .AnyAsync(t => t.AccountId == entity.AccountId);
            if (hasTransaction)
                return "امکان حذف شخص وجود ندارد، زیرا تراکنش‌های مالی برای آن ثبت شده است.";

            // Soft Delete حساب مالی
            if (entity.Account != null)
                entity.Account.IsDelete = true;

            // Soft Delete شخص
            entity.IsDelete = true;

            // ثبت لاگ
            await _context.LogUser.AddAsync(new BusinessEntity.Settings.LogUser
            {
                Description = $"غیرفعال‌سازی شخص ({entity.FirstName} {entity.LastName}) و حساب مالی مرتبط",
                UserId = userId,
                Date = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return "عملیات با موفقیت انجام شد.";
        }

    }
}
