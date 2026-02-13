using BusinessEntity.Customer_Club;
using BusinessEntity.People;
using BusinessEntity.Product;
using BusinessEntity.Settings;
using DataAccessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options) : base(options)
        {
        }

        // ========== Customer_Club ==========
        public DbSet<BusinessEntity.Customer_Club.ClubDiscount> ClubDiscount { get; set; } = null!;
        public DbSet<BusinessEntity.Customer_Club.ClubDiscountProduct> ClubDiscountProduct { get; set; } = null!;
        public DbSet<BusinessEntity.Customer_Club.Customer> Customer { get; set; } = null!;
        public DbSet<BusinessEntity.Customer_Club.CustomerLevel> CustomerLevel { get; set; } = null!;
        public DbSet<BusinessEntity.Customer_Club.CustomerLevelHistory> CustomerLevelHistory { get; set; } = null!;
        public DbSet<BusinessEntity.Customer_Club.PointTransaction> PointTransaction { get; set; } = null!;
        public DbSet<BusinessEntity.Customer_Club.PublicDiscount> PublicDiscount { get; set; } = null!;
        public DbSet<BusinessEntity.Customer_Club.PublicDiscountProduct> PublicDiscountProduct { get; set; } = null!;
        public DbSet<BusinessEntity.Customer_Club.Store> Store { get; set; } = null!; // ✅ اضافه شد
        public DbSet<BusinessEntity.Customer_Club.Wallet> Wallet { get; set; } = null!;
        public DbSet<BusinessEntity.Customer_Club.WalletTransaction> WalletTransaction { get; set; } = null!;

        // ========== Bank ==========
        public DbSet<BusinessEntity.Fund.Definition_Bank> Definition_Bank { get; set; } = null!;
        public DbSet<BusinessEntity.Fund.Definition_Bank_Account> Definition_Bank_Account { get; set; } = null!;

        // ========== Financial_Operations ==========
        public DbSet<BusinessEntity.Invoices.Account> Account { get; set; } = null!;
        public DbSet<BusinessEntity.Invoices.Transaction> Transaction { get; set; } = null!;

        // ========== Fund ==========
        public DbSet<BusinessEntity.Fund.Fund> Fund { get; set; } = null!;
        public DbSet<BusinessEntity.Fund.Cash_Register_To_The_User> Cash_Register_To_The_User { get; set; } = null!;
        public DbSet<BusinessEntity.Fund.Work_Shift> Work_Shift { get; set; } = null!;

        // ========== Invoices ==========
        public DbSet<BusinessEntity.Invoices.Invoices> Invoices { get; set; } = null!;
        public DbSet<BusinessEntity.Invoices.Invoices_Item> Invoices_Item { get; set; } = null!;

        // ========== People ==========
        public DbSet<BusinessEntity.People.Type_People> Type_People { get; set; } = null!;
        public DbSet<BusinessEntity.People.Group_People> Group_People { get; set; } = null!;
        public DbSet<BusinessEntity.People.People> People { get; set; } = null!;

        // ========== Product ==========
        public DbSet<BusinessEntity.Product.Group_Product> Group_Product { get; set; } = null!;
        public DbSet<BusinessEntity.Product.PriceLevels> PriceLevels { get; set; } = null!;
        public DbSet<BusinessEntity.Product.Product_Failure_Item> Product_Failure_Item { get; set; } = null!;
        public DbSet<BusinessEntity.Product.Product_Failure> Product_Failure { get; set; } = null!;
        public DbSet<BusinessEntity.Product.Product> Product { get; set; } = null!;
        public DbSet<BusinessEntity.Product.ProductBarcodes> ProductBarcodes { get; set; } = null!;
        public DbSet<BusinessEntity.Product.ProductPrices> ProductPrices { get; set; } = null!;
        public DbSet<BusinessEntity.Product.Section_Product> Section_Product { get; set; } = null!;
        public DbSet<BusinessEntity.Product.Storeroom_Product> Storeroom_Product { get; set; } = null!;
        public DbSet<BusinessEntity.Product.Type_Product> Type_Product { get; set; } = null!;
        public DbSet<BusinessEntity.Product.Unit_Product> Unit_Product { get; set; } = null!;
        public DbSet<BusinessEntity.Product.UnitsLevel> UnitsLevel { get; set; } = null!;

        // ========== Settings ==========
        public DbSet<BusinessEntity.Settings.Access_Level> Access_Level { get; set; } = null!;
        public DbSet<BusinessEntity.Settings.Group_User> Group_User { get; set; } = null!;
        public DbSet<BusinessEntity.Settings.LogUser> LogUser { get; set; } = null!;
        public DbSet<BusinessEntity.Settings.Reminder> Reminder { get; set; } = null!;
        public DbSet<BusinessEntity.Settings.User> User { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ✅ فقط یکبار

            // اعمال کانفیگ‌های هر موجودیت
            ConfigureCustomer(modelBuilder);
            ConfigureWallet(modelBuilder);
            ConfigureClubDiscount(modelBuilder);
            ConfigurePublicDiscount(modelBuilder);
            ConfigureCustomerLevel(modelBuilder);
            ConfigurePointTransaction(modelBuilder);

            modelBuilder.Entity<BusinessEntity.Fund.Definition_Bank>(entity =>
            {
                entity.ToTable("definition_bank");
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Name)
                      .HasColumnName("name")
                      .HasMaxLength(30)
                      .IsRequired();
            });

            modelBuilder.Entity<BusinessEntity.Fund.Definition_Bank_Account>(entity =>
            {
                entity.ToTable("definition_bank_account");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.AccountNumber)
                      .HasColumnName("account_number")
                      .HasMaxLength(20);
                entity.Property(a => a.TypeAccount)
                      .HasColumnName("type_account")
                      .HasMaxLength(20);
                entity.Property(a => a.PeopleAccount)
                      .HasColumnName("people_account")
                      .HasMaxLength(50);
                entity.Property(a => a.CardNumber)
                      .HasColumnName("card_number")
                      .HasMaxLength(16);
                entity.HasOne(a => a.Bank)
                      .WithMany(b => b.BankAccounts)
                      .HasForeignKey(a => a.BankId);
            });

            modelBuilder.Entity<BusinessEntity.Invoices.Account>()
                .HasIndex(a => a.AccountName)
                .IsUnique();

            modelBuilder.Entity<BusinessEntity.Invoices.Account>()
                .Property(a => a.Balance)
                .HasColumnType("numeric(18,2)");

            modelBuilder.Entity<BusinessEntity.Invoices.Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("numeric(18,2)");

            modelBuilder.Entity<BusinessEntity.Fund.Cash_Register_To_The_User>()
                .HasOne(c => c.User)
                .WithMany(u => u.CashRegisters)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BusinessEntity.Fund.Cash_Register_To_The_User>()
                .HasOne(c => c.Fund)
                .WithMany(f => f.CashRegisters)
                .HasForeignKey(c => c.FundId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BusinessEntity.Fund.Work_Shift>()
                .HasOne(w => w.CashRegisterToUser)
                .WithMany(c => c.WorkShifts)
                .HasForeignKey(w => w.CashRegisterToUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Invoices
            modelBuilder.Entity<BusinessEntity.Invoices.Invoices>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BusinessEntity.Invoices.Invoices>()
                .HasOne(i => i.People)
                .WithMany(p => p.Invoices)
                .HasForeignKey(i => i.PeopleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BusinessEntity.Invoices.Invoices>()
                .HasOne(i => i.User)
                .WithMany(u => u.Invoices)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BusinessEntity.Invoices.Invoices_Item>()
                .HasOne(ii => ii.Invoices)
                .WithMany(i => i.Invoices_Item)
                .HasForeignKey(ii => ii.InvoicesId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BusinessEntity.Invoices.Invoices_Item>()
                .HasOne(ii => ii.Product)
                .WithMany(p => p.Invoices_Items)
                .HasForeignKey(ii => ii.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // People
            modelBuilder.Entity<People>()
                .HasOne(a => a.Group_People)
                .WithMany(b => b.People)
                .HasForeignKey(a => a.GroupPeopleId);

            modelBuilder.Entity<People>()
                .HasOne(a => a.Type_People)
                .WithMany(b => b.People)
                .HasForeignKey(a => a.TypePeopleId);

            modelBuilder.Entity<People>()
                .HasOne(a => a.PriceLevel)
                .WithMany(b => b.People)
                .HasForeignKey(a => a.PriceLevelID);

            // Product
            modelBuilder.Entity<Storeroom_Product>()
                .HasOne(a => a.Section_Product)
                .WithMany(b => b.Storeroom_Product)
                .HasForeignKey(a => a.SectionProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Storeroom_Product>()
                .HasOne(a => a.People)
                .WithMany(b => b.Storeroom_Product)
                .HasForeignKey(a => a.PeopleId);

            modelBuilder.Entity<Product>()
                .HasOne(a => a.GroupProduct)
                .WithMany(b => b.Products)
                .HasForeignKey(a => a.GroupProductId);

            modelBuilder.Entity<Product>()
                .HasOne(a => a.SectionProduct)
                .WithMany(b => b.Products)
                .HasForeignKey(a => a.SectionProductId);

            modelBuilder.Entity<Product>()
                .HasOne(a => a.TypeProduct)
                .WithMany(b => b.Products)
                .HasForeignKey(a => a.TypeProductId);

            modelBuilder.Entity<Product>()
                .HasOne(a => a.Unit_Product)
                .WithMany(b => b.Products)
                .HasForeignKey(a => a.UnitProductId);

            modelBuilder.Entity<Product>()
                .HasOne(a => a.StoreroomProduct)
                .WithMany(b => b.Products)
                .HasForeignKey(a => a.StoreroomProductId);

            // UnitsLevel
            modelBuilder.Entity<UnitsLevel>()
                .HasOne(a => a.Product)
                .WithMany(b => b.Units)
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UnitsLevel>()
                .HasOne(a => a.UnitProduct)
                .WithMany(b => b.UnitsLevel)
                .HasForeignKey(a => a.UnitProductId);

            // ProductPrices
            modelBuilder.Entity<ProductPrices>()
                .HasOne(a => a.ProductUnit)
                .WithMany(b => b.Prices)
                .HasForeignKey(a => a.UnitLevelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductPrices>()
                .HasOne(a => a.PriceLevel)
                .WithMany(b => b.ProductPrices)
                .HasForeignKey(a => a.PriceLevelId);

            // ProductBarcodes
            modelBuilder.Entity<ProductBarcodes>()
                .HasOne(a => a.ProductUnit)
                .WithMany(b => b.Barcodes)
                .HasForeignKey(a => a.ProductUnitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductBarcodes>()
                .Property(b => b.Barcode)
                .HasMaxLength(60)
                .IsRequired();

            // Decimal precision
            modelBuilder.Entity<ProductPrices>()
                .Property(p => p.BuyPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ProductPrices>()
                .Property(p => p.Profit).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ProductPrices>()
                .Property(p => p.SalePrice).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.BuyPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Product>()
                .Property(p => p.Profit).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Product>()
                .Property(p => p.SalePrice).HasColumnType("decimal(18,2)");

            // Settings
            modelBuilder.Entity<User>()
                .HasOne(u => u.Group_User)
                .WithMany(g => g.Users)
                .HasForeignKey(u => u.GroupUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Group_User>()
                .HasOne(g => g.AccessLevel)
                .WithOne(a => a.Group_User)
                .HasForeignKey<Access_Level>(a => a.GroupUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(a => a.People)
                .WithMany(b => b.Users)
                .HasForeignKey(a => a.PeopleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LogUser>()
                .HasOne(a => a.User)
                .WithMany(b => b.LogUser)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reminder>()
                .HasOne(a => a.User)
                .WithMany(b => b.Reminders)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            // Soft Delete Query Filter
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ModelBuilder)
                        .GetMethod(nameof(ModelBuilder.Entity), 1, Type.EmptyTypes)!
                        .MakeGenericMethod(entityType.ClrType);

                    var builder = method.Invoke(modelBuilder, null);

                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var body = Expression.Equal(
                        Expression.Property(parameter, nameof(ISoftDelete.IsDelete)),
                        Expression.Constant(false));

                    var lambda = Expression.Lambda(body, parameter);

                    builder!.GetType()
                        .GetMethod("HasQueryFilter")!
                        .Invoke(builder, new object[] { lambda });
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
                       Microsoft.Extensions.Logging.LogLevel.Information);
        }

        public override int SaveChanges()
        {
            SoftDeleteInterceptor();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SoftDeleteInterceptor();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SoftDeleteInterceptor()
        {
            var deletedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted &&
                            e.Entity is ISoftDelete);

            foreach (var entry in deletedEntries)
            {
                var entity = (ISoftDelete)entry.Entity;
                entity.IsDelete = true;
                entry.State = EntityState.Modified;
            }
        }

        // =============================
        // Configurations
        // =============================

        private void ConfigureCustomer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Mobile).IsUnique();
                entity.HasIndex(e => e.Barcode).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasMaxLength(12);

                entity.Property(e => e.Email)
                    .HasMaxLength(80);

                entity.Property(e => e.TotalPurchaseAmount)
                    .HasColumnType("decimal(18,2)");

                // ارتباط با Wallet
                entity.HasOne(e => e.Wallet)
                    .WithOne(w => w.Customer)
                    .HasForeignKey<Wallet>(w => w.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                // ارتباط با CustomerLevel
                entity.HasOne(e => e.CustomerLevel)
                    .WithMany(cl => cl.Customers)
                    .HasForeignKey(e => e.CustomerLevelId)
                    .OnDelete(DeleteBehavior.SetNull);

                // ارتباط با Store
                entity.HasOne(e => e.Store)
                    .WithMany(s => s.Customers)
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.Restrict);

                // ✅ ارتباط با People (جدید)
                entity.HasOne(e => e.People)
                    .WithMany()
                    .HasForeignKey(e => e.PeopleId)
                    .OnDelete(DeleteBehavior.SetNull);

                // ارتباط با LevelHistories
                entity.HasMany(e => e.LevelHistories)
                    .WithOne(clh => clh.Customer)
                    .HasForeignKey(clh => clh.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                // ارتباط با PointTransactions
                entity.HasMany(e => e.PointTransactions)
                    .WithOne(pt => pt.Customer)
                    .HasForeignKey(pt => pt.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureWallet(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Balance)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.HasOne(e => e.Customer)
                    .WithOne(c => c.Wallet)
                    .HasForeignKey<Wallet>(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Transactions)
                    .WithOne(wt => wt.Wallet)
                    .HasForeignKey(wt => wt.WalletId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WalletTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.HasOne(e => e.Wallet)
                    .WithMany(w => w.Transactions)
                    .HasForeignKey(e => e.WalletId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Invoice)
                    .WithMany(i => i.WalletTransactions)
                    .HasForeignKey(e => e.InvoiceId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.ClubDiscount)
                    .WithMany()
                    .HasForeignKey(e => e.ClubDiscountId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }

        private void ConfigureClubDiscount(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClubDiscount>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.Value)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Store)
                    .WithMany(s => s.ClubDiscounts)
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Products)
                    .WithOne(p => p.ClubDiscount)
                    .HasForeignKey(p => p.ClubDiscountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ClubDiscountProduct>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ClubPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.OriginalPrice)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.ClubDiscount)
                    .WithMany(cd => cd.Products)
                    .HasForeignKey(e => e.ClubDiscountId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                // ✅ اضافه کردن ارتباط با UnitLevel
                entity.HasOne(e => e.UnitLevel)
                    .WithMany()
                    .HasForeignKey(e => e.UnitLevelId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigurePublicDiscount(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PublicDiscount>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.Value)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Store)
                    .WithMany(s => s.PublicDiscounts)
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Products)
                    .WithOne(p => p.PublicDiscount)
                    .HasForeignKey(p => p.PublicDiscountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PublicDiscountProduct>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DiscountedPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.OriginalPrice)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.PublicDiscount)
                    .WithMany(pd => pd.Products)
                    .HasForeignKey(e => e.PublicDiscountId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                // ✅ اضافه کردن ارتباط با UnitLevel
                entity.HasOne(e => e.UnitLevel)
                    .WithMany()
                    .HasForeignKey(e => e.UnitLevelId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureCustomerLevel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerLevel>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.HasOne(e => e.Store)
                    .WithMany(s => s.CustomerLevels)
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Customers)
                    .WithOne(c => c.CustomerLevel)
                    .HasForeignKey(c => c.CustomerLevelId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(e => e.LevelHistories)
                    .WithOne(clh => clh.CustomerLevel)
                    .HasForeignKey(clh => clh.CustomerLevelId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CustomerLevelHistory>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.LevelHistories)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.CustomerLevel)
                    .WithMany(cl => cl.LevelHistories)
                    .HasForeignKey(e => e.CustomerLevelId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigurePointTransaction(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PointTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.PointTransactions)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Invoice)
                    .WithMany(i => i.PointTransactions)
                    .HasForeignKey(e => e.InvoiceId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}


