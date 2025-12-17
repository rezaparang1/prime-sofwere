using BusinessEntity.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class DbInitializer
    {
        private readonly Database _db;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(Database db, ILogger<DbInitializer> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await _db.Database.MigrateAsync();
            _logger.LogInformation("Seeding initial data...");

            // -----------------------
            // گروه کاربری
            // -----------------------
            var groupUser = await _db.Set<BusinessEntity.Settings.Group_User>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(g => g.Name == "مدیر");

            if (groupUser != null)
                groupUser.IsDelete = false;
            else
            {
                groupUser = new BusinessEntity.Settings.Group_User { Name = "مدیر", IsDelete = false };
                _db.Add(groupUser);
            }
            await _db.SaveChangesAsync();

            // -----------------------
            // نوع محصولات
            // -----------------------
            var productTypes = new[] { "کالا", "خدمات", "دارایی های ثابت" }
                .Select(name => new BusinessEntity.Product.Type_Product { Name = name, IsDelete = false }).ToList();

            foreach (var pt in productTypes)
            {
                var existing = await _db.Set<BusinessEntity.Product.Type_Product>()
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(x => x.Name == pt.Name);

                if (existing != null)
                    existing.IsDelete = false;
                else
                    _db.Add(pt);
            }
            await _db.SaveChangesAsync();

            // -----------------------
            // گروه محصول
            // -----------------------
            var groupProduct = await _db.Set<BusinessEntity.Product.Group_Product>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(g => g.Name == "پیش فرض");

            if (groupProduct != null)
                groupProduct.IsDelete = false;
            else
            {
                groupProduct = new BusinessEntity.Product.Group_Product { Name = "پیش فرض", IsDelete = false };
                _db.Add(groupProduct);
            }
            await _db.SaveChangesAsync();

            // -----------------------
            // واحدهای محصول
            // -----------------------
            var unitNames = new[] { "عدد", "بسته", "کیلوگرم", "متر", "مثقال", "کارتن", "پرس", "شل", "متر مربع", "تن", "شاخه", "ورق" };
            foreach (var name in unitNames)
            {
                var unit = await _db.Set<BusinessEntity.Product.Unit_Product>()
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(u => u.Name == name);
                if (unit != null)
                    unit.IsDelete = false;
                else
                    _db.Add(new BusinessEntity.Product.Unit_Product { Name = name, IsDelete = false });
            }
            await _db.SaveChangesAsync();

            // -----------------------
            // بخش محصول
            // -----------------------
            var sectionProduct = await _db.Set<BusinessEntity.Product.Section_Product>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(s => s.Name == "پیش فرض");

            if (sectionProduct != null)
                sectionProduct.IsDelete = false;
            else
                _db.Add(new BusinessEntity.Product.Section_Product { Name = "پیش فرض", IsDelete = false });

            await _db.SaveChangesAsync();

            // -----------------------
            // سطوح قیمت
            // -----------------------
            var priceLevelsNames = new[] { "مشتری", "همکار", "دوستان" };
            var priceLevels = new List<BusinessEntity.Product.PriceLevels>();
            foreach (var name in priceLevelsNames)
            {
                var pl = await _db.Set<BusinessEntity.Product.PriceLevels>()
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(p => p.Name == name);
                if (pl != null)
                {
                    pl.IsDelete = false;
                    priceLevels.Add(pl);
                }
                else
                {
                    pl = new BusinessEntity.Product.PriceLevels { Name = name, IsDelete = false };
                    _db.Add(pl);
                    priceLevels.Add(pl);
                }
            }
            await _db.SaveChangesAsync();

            // -----------------------
            // گروه و نوع اشخاص
            // -----------------------
            var groupPeople = await _db.Set<BusinessEntity.People.Group_People>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(g => g.Name == "پیش فرض");

            if (groupPeople != null)
                groupPeople.IsDelete = false;
            else
                _db.Add(new BusinessEntity.People.Group_People { Name = "پیش فرض", IsDelete = false });

            var typePeople1 = await _db.Set<BusinessEntity.People.Type_People>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(t => t.Name == "حقیقی");
            if (typePeople1 != null)
                typePeople1.IsDelete = false;
            else
                _db.Add(new BusinessEntity.People.Type_People { Name = "حقیقی", IsDelete = false });

            var typePeople2 = await _db.Set<BusinessEntity.People.Type_People>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(t => t.Name == "حقوقی");
            if (typePeople2 != null)
                typePeople2.IsDelete = false;
            else
                _db.Add(new BusinessEntity.People.Type_People { Name = "حقوقی", IsDelete = false });

            await _db.SaveChangesAsync();

            // -----------------------
            // بانک‌ها
            // -----------------------
            var bankNames = new[] { "ملت", "ملی", "تجارت", "کشاورزی", "صادرات", "رفاه", "سامان", "پارسیان", "سپه", "مسکن" };
            foreach (var name in bankNames)
            {
                var bank = await _db.Set<BusinessEntity.Bank.Definition_Bank>()
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(b => b.Name == name);
                if (bank != null)
                    bank.IsDelete = false;
                else
                    _db.Add(new BusinessEntity.Bank.Definition_Bank { Name = name, IsDelete = false });
            }
            await _db.SaveChangesAsync();

            // -----------------------
            // Accountها
            // -----------------------
            var fundAccount = await _db.Set<BusinessEntity.Financial_Operations.Account>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.AccountName == "مدیر");

            if (fundAccount != null)
                fundAccount.IsDelete = false;
            else
            {
                fundAccount = new BusinessEntity.Financial_Operations.Account
                {
                    AccountName = "مدیر",
                    AccountType = "CashBox",
                    Balance = 0,
                    IsDelete = false
                };
                _db.Add(fundAccount);
            }

            var peopleAccount = await _db.Set<BusinessEntity.Financial_Operations.Account>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.AccountName == "مشتری پیش فرض");

            if (peopleAccount != null)
                peopleAccount.IsDelete = false;
            else
            {
                peopleAccount = new BusinessEntity.Financial_Operations.Account
                {
                    AccountName = "مشتری پیش فرض",
                    AccountType = "People",
                    Balance = 0,
                    IsDelete = false
                };
                _db.Add(peopleAccount);
            }

            await _db.SaveChangesAsync(); // Save Accounts قبل از Fund و People

            // -----------------------
            // Fund
            // -----------------------
            var fund = await _db.Set<BusinessEntity.Fund.Fund>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(f => f.Name == "مدیر");

            if (fund != null)
                fund.IsDelete = false;
            else
            {
                fund = new BusinessEntity.Fund.Fund
                {
                    Name = "مدیر",
                    FirstInventory = 0,
                    Inventory = 0,
                    NegativeBalancePolicy = BusinessEntity.Fund.NegativeBalancePolicy.No,
                    Account = fundAccount,
                    IsDelete = false
                };
                _db.Add(fund);
            }

            // -----------------------
            // People
            // -----------------------
            var people = await _db.Set<BusinessEntity.People.People>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.IdPeople == "1");

            if (people != null)
                people.IsDelete = false;
            else
            {
                people = new BusinessEntity.People.People
                {
                    IdPeople = "1",
                    FirstName = "مشتری",
                    LastName = "پیش فرض",
                    Type_People = typePeople1,
                    CreditLimit = 0,
                    IsCreditLimit = false,
                    HowToDoBusiness = BusinessEntity.People.HowToDoBusiness.BothOfThem,
                    OFF = 0,
                    Business = true,
                    User = true,
                    Employee = true,
                    Investor = false,
                    TaxFree = false,
                    InitialCapital = 0,
                    Inventory = 0,
                    Group_People = groupPeople,
                    PriceLevel = priceLevels[0],
                    Account = peopleAccount,
                    IsDelete = false
                };
                _db.Add(people);
            }

            await _db.SaveChangesAsync();

            // -----------------------
            // انبار
            // -----------------------
            var sectionProductDefault = await _db.Set<BusinessEntity.Product.Section_Product>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(s => s.Name == "پیش فرض");

            var anbarProduct = await _db.Set<BusinessEntity.Product.Storeroom_Product>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Name == "پیش فرض");

            if (anbarProduct != null)
                anbarProduct.IsDelete = false;
            else
                _db.Add(new BusinessEntity.Product.Storeroom_Product
                {
                    Section_Product = sectionProductDefault,
                    Name = "پیش فرض",
                    People = people,
                    Description = "انبار مرکزی",
                    NegativeBalancePolicy = BusinessEntity.Product.NegativeBalancePolicy.No,
                    IsDelete = false
                });

            await _db.SaveChangesAsync();

            // -----------------------
            // دسترسی‌ها
            // -----------------------
            var access = await _db.Set<BusinessEntity.Settings.Access_Level>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.GroupUserId == groupUser.Id);

            if (access != null)
                access.IsDelete = false;
            else
                _db.Add(new BusinessEntity.Settings.Access_Level
                {
                    Group_User = groupUser,
                    IsBankT = true,
                    IsBank = true,
                    IsFund = true,
                    IsWorkShift = true,
                    IsRegisterUser = true,
                    IsInvoices = true,
                    IsPeople = true,
                    IsGroupPeople = true,
                    IsTypePeopel = true,
                    IsGroupProduct = true,
                    IsPriceLevel = true,
                    IsProductFailre = true,
                    IsProduct = true,
                    IsSectionProduct = true,
                    IsTypeProduct = true,
                    IsStoreroomProduct = true,
                    IsUnitProduct = true,
                    IsGroupUser = true,
                    IsUser = true,
                    IsAccessLevel = true,
                    IsViewingofOthers = true,
                    IsDelete = false
                });

            await _db.SaveChangesAsync();

            // -----------------------
            // کاربر admin
            // -----------------------
            var admin = await _db.Set<User>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.UserName == "admin");

            if (admin != null)
            {
                admin.IsDelete = false;
                admin.IsActive = true;
                admin.Group_User = groupUser;
                admin.People = people;
            }
            else
            {
                _db.Add(new User
                {
                    UserName = "admin",
                    Password = PasswordHasher.Hash("123456"),
                    Group_User = groupUser,
                    People = people,
                    IsDelete = false,
                    IsActive = true
                });
            }

            await _db.SaveChangesAsync();

            _logger.LogInformation("Seeding completed. Admin user: admin / 123456, Bank: ملت, Fund: صندوق اصلی");
        }
    }
}
