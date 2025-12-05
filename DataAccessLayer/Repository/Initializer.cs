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

            if (await _db.Set<User>().AnyAsync())
            {
                _logger.LogInformation("Database already seeded.");
                return;
            }

            _logger.LogInformation("Seeding initial data...");

            // -----------------------
            // گروه کاربری
            // -----------------------
            var groupUser = await _db.Set<BusinessEntity.Settings.Group_User>()
                .FirstOrDefaultAsync(g => g.Name == "مدیر")
                ?? new BusinessEntity.Settings.Group_User { Name = "مدیر", IsDelete = false };
            if (_db.Entry(groupUser).State == EntityState.Detached)
                _db.Add(groupUser);

            // -----------------------
            // نوع محصولات
            // -----------------------
            var productTypes = new[]
            {
            new BusinessEntity.Product.Type_Product { Name = "کالا", IsDelete = false },
            new BusinessEntity.Product.Type_Product { Name = "خدمات", IsDelete = false },
            new BusinessEntity.Product.Type_Product { Name = "دارایی های ثابت", IsDelete = false }
        };
            foreach (var pt in productTypes)
            {
                if (!await _db.Set<BusinessEntity.Product.Type_Product>().AnyAsync(x => x.Name == pt.Name))
                    _db.Add(pt);
            }

            // -----------------------
            // گروه محصول
            // -----------------------
            var groupProduct = await _db.Set<BusinessEntity.Product.Group_Product>()
                .FirstOrDefaultAsync(g => g.Name == "پیش فرض")
                ?? new BusinessEntity.Product.Group_Product { Name = "پیش فرض", IsDelete = false };
            if (_db.Entry(groupProduct).State == EntityState.Detached)
                _db.Add(groupProduct);

            // -----------------------
            // واحدهای محصول
            // -----------------------
            var unitProducts = new[]
            {
            "عدد","بسته","کیلوگرم","متر","مثقال","کارتن","پرس","شل","متر مربع","تن","شاخه","ورق"
        }.Select(u => new BusinessEntity.Product.Unit_Product { Name = u, IsDelete = false }).ToList();

            foreach (var u in unitProducts)
            {
                if (!await _db.Set<BusinessEntity.Product.Unit_Product>().AnyAsync(x => x.Name == u.Name))
                    _db.Add(u);
            }

            // -----------------------
            // بخش محصول
            // -----------------------
            var sectionProduct = await _db.Set<BusinessEntity.Product.Section_Product>()
                .FirstOrDefaultAsync(s => s.Name == "پیش فرض")
                ?? new BusinessEntity.Product.Section_Product { Name = "پیش فرض", IsDelete = false };
            if (_db.Entry(sectionProduct).State == EntityState.Detached)
                _db.Add(sectionProduct);

            // -----------------------
            // سطوح قیمت
            // -----------------------
            var priceLevels = new[]
            {
            "مشتری","همکار","دوستان"
        }.Select(p => new BusinessEntity.Product.PriceLevels { Name = p, IsDelete = false }).ToList();
            foreach (var p in priceLevels)
            {
                if (!await _db.Set<BusinessEntity.Product.PriceLevels>().AnyAsync(x => x.Name == p.Name))
                    _db.Add(p);
            }

            // -----------------------
            // گروه و نوع اشخاص
            // -----------------------
            var groupPeople = await _db.Set<BusinessEntity.People.Group_People>()
                .FirstOrDefaultAsync(g => g.Name == "پیش فرض")
                ?? new BusinessEntity.People.Group_People { Name = "پیش فرض", IsDelete = false };
            if (_db.Entry(groupPeople).State == EntityState.Detached)
                _db.Add(groupPeople);

            var typePeople1 = await _db.Set<BusinessEntity.People.Type_People>()
                .FirstOrDefaultAsync(t => t.Name == "حقیقی")
                ?? new BusinessEntity.People.Type_People { Name = "حقیقی", IsDelete = false };
            if (_db.Entry(typePeople1).State == EntityState.Detached)
                _db.Add(typePeople1);

            var typePeople2 = await _db.Set<BusinessEntity.People.Type_People>()
                .FirstOrDefaultAsync(t => t.Name == "حقوقی")
                ?? new BusinessEntity.People.Type_People { Name = "حقوقی", IsDelete = false };
            if (_db.Entry(typePeople2).State == EntityState.Detached)
                _db.Add(typePeople2);

            // -----------------------
            // بانک‌ها
            // -----------------------
            var banksNames = new[] { "ملت", "ملی", "تجارت", "کشاورزی", "صادرات", "رفاه", "سامان", "پارسیان", "سپه", "مسکن" };
            foreach (var name in banksNames)
            {
                if (!await _db.Set<BusinessEntity.Bank.Definition_Bank>().AnyAsync(b => b.Name == name))
                    _db.Add(new BusinessEntity.Bank.Definition_Bank { Name = name, IsDelete = false });
            }

            await _db.SaveChangesAsync(); // ذخیره همه parentها قبل از FKها

            // -----------------------
            // Account و Fund
            // -----------------------
            var fundAccount = await _db.Set<BusinessEntity.Financial_Operations.Account>()
                .FirstOrDefaultAsync(a => a.AccountName == "مدیر")
                ?? new BusinessEntity.Financial_Operations.Account
                {
                    AccountName = "مدیر",
                    AccountType = "CashBox",
                    Balance = 0
                };

            if (_db.Entry(fundAccount).State == EntityState.Detached)
                _db.Add(fundAccount);

            var fund = await _db.Set<BusinessEntity.Fund.Fund>()
                .FirstOrDefaultAsync(f => f.Name == "مدیر")
                ?? new BusinessEntity.Fund.Fund
                {
                    Name = "مدیر",
                    FirstInventory = 0,
                    Inventory = 0,
                    IsDelete = false,
                    NegativeBalancePolicy = BusinessEntity.Fund.NegativeBalancePolicy.No,
                    Account = fundAccount
                };
            if (_db.Entry(fund).State == EntityState.Detached)
                _db.Add(fund);

            // -----------------------
            // Account و People
            // -----------------------
            var peopleAccount = await _db.Set<BusinessEntity.Financial_Operations.Account>()
                .FirstOrDefaultAsync(a => a.AccountName == "مشتری پیش فرض")
                ?? new BusinessEntity.Financial_Operations.Account
                {
                    AccountName = "مشتری پیش فرض",
                    AccountType = "People",
                    Balance = 0
                };
            if (_db.Entry(peopleAccount).State == EntityState.Detached)
                _db.Add(peopleAccount);

            var people = await _db.Set<BusinessEntity.People.People>()
                .FirstOrDefaultAsync(p => p.IdPeople == "1")
                ?? new BusinessEntity.People.People
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
                    Account = peopleAccount
                };
            if (_db.Entry(people).State == EntityState.Detached)
                _db.Add(people);

            // -----------------------
            // انبار
            // -----------------------
            var anbarProduct = await _db.Set<BusinessEntity.Product.Storeroom_Product>()
                .FirstOrDefaultAsync(a => a.Name == "پیش فرض")
                ?? new BusinessEntity.Product.Storeroom_Product
                {
                    Section_Product = sectionProduct,
                    Name = "پیش فرض",
                    People = people,
                    Description = "انبار مرکزی",
                    NegativeBalancePolicy = BusinessEntity.Product.NegativeBalancePolicy.No
                };
            if (_db.Entry(anbarProduct).State == EntityState.Detached)
                _db.Add(anbarProduct);

            // -----------------------
            // دسترسی‌ها
            // -----------------------
            var access = new Access_Level
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
                IsViewingofOthers = true
            };
            _db.Add(access);

            // -----------------------
            // کاربر admin
            // -----------------------
            var admin = await _db.Set<User>()
                .FirstOrDefaultAsync(u => u.UserName == "admin")
                ?? new User
                {
                    People = people,
                    UserName = "admin",
                    Password = PasswordHasher.Hash("123456"),
                    IsDelete = false,
                    IsActive = true,
                    Group_User = groupUser
                };
            if (_db.Entry(admin).State == EntityState.Detached)
                _db.Add(admin);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Seeding completed. Admin user: admin / 123456, Bank: ملت, Fund: صندوق اصلی");
        }
    }

}
