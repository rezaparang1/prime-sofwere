using BusinessEntity;
using BusinessEntity.Bank;
using BusinessEntity.Financial_Operations;
using BusinessEntity.People;
using BusinessEntity.Product;
using BusinessEntity.Settings;
using DataAccessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options) : base(options)
        {
            //optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }
        //Customer_Club
        public DbSet<BusinessEntity.Customer_Club.Customer> Customer { get; set; } = null!;
        //Bank
        public DbSet<BusinessEntity.Bank.Definition_Bank> Definition_Bank { get; set; } = null!;
        public DbSet<BusinessEntity.Bank.Definition_Bank_Account> Definition_Bank_Account { get; set; } = null!;
        public DbSet<BusinessEntity.Bank.Bank_To_Bank> Bank_To_Bank { get; set; } = null!;
        public DbSet<BusinessEntity.Bank.Pay_To_Bank> Pay_To_Bank { get; set; } = null!;
        //Financial_Operations
        public DbSet<BusinessEntity.Financial_Operations.Account> Account { get; set; } = null!;
        public DbSet<BusinessEntity.Financial_Operations.Transaction> Transaction { get; set; } = null!;
        //Fund
        public DbSet<BusinessEntity.Fund.Fund> Fund { get; set; } = null!;
        public DbSet<BusinessEntity.Fund.Cash_Register_To_The_User> Cash_Register_To_The_User { get; set; } = null!;
        public DbSet<BusinessEntity.Fund.Work_Shift> Work_Shift { get; set; } = null!;
        //Invoices
        public DbSet<BusinessEntity.Invoices.Invoices> Invoices { get; set; } = null!;
        public DbSet<BusinessEntity.Invoices.Invoices_Item> Invoices_Item { get; set; } = null!;
        //People
        public DbSet<BusinessEntity.People.Type_People> Type_People { get; set; } = null!;
        public DbSet<BusinessEntity.People.Group_People> Group_People { get; set; } = null!;
        public DbSet<BusinessEntity.People.People> People { get; set; } = null!;
        //Product
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
        //Settigs
        public DbSet<BusinessEntity.Settings.Access_Level> Access_Level { get; set; } = null!;
        public DbSet<BusinessEntity.Settings.Group_User> Group_User { get; set; } = null!;
        public DbSet<BusinessEntity.Settings.LogUser> LogUser { get; set; } = null!;
        public DbSet<BusinessEntity.Settings.Reminder> Reminder { get; set; } = null!;
        public DbSet<BusinessEntity.Settings.User> User { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Definition_Bank>(entity =>
            {
                entity.ToTable("definition_bank"); 
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Name)
                      .HasColumnName("name")
                      .HasMaxLength(30)
                      .IsRequired();
            });
            modelBuilder.Entity<Definition_Bank_Account>(entity =>
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
                modelBuilder.Entity<BusinessEntity.Bank.Definition_Bank_Account>()
                 .HasOne(a => a.Bank)
                 .WithMany(b => b.BankAccounts)
                 .HasForeignKey(a => a.BankId);
            });
            modelBuilder.Entity<Account>()
            .HasIndex(a => a.AccountName)
            .IsUnique();

            // تنظیم نوع عددی برای PostgreSQL
            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasColumnType("numeric(18,2)");

            modelBuilder.Entity<Transaction>()
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


            modelBuilder.Entity<BusinessEntity.Invoices.Invoices>()
         .HasOne(i => i.Customer)
         .WithMany(c => c.Invoices)
         .HasForeignKey(i => i.CustomerId)
         .OnDelete(DeleteBehavior.Restrict);

            // Invoices → People
            modelBuilder.Entity<BusinessEntity.Invoices.Invoices>()
                .HasOne(i => i.People)
                .WithMany(p => p.Invoices)
                .HasForeignKey(i => i.PeopleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoices → User
            modelBuilder.Entity<BusinessEntity.Invoices.Invoices>()
                .HasOne(i => i.User)
                .WithMany(u => u.Invoices)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoices_Item → Invoices
            modelBuilder.Entity<BusinessEntity.Invoices.Invoices_Item>()
                .HasOne(ii => ii.Invoices)
                .WithMany(i => i.Invoices_Item)
                .HasForeignKey(ii => ii.InvoicesId)
                .OnDelete(DeleteBehavior.Cascade);

            // Invoices_Item → Product
            modelBuilder.Entity<BusinessEntity.Invoices.Invoices_Item>()
                .HasOne(ii => ii.Product)
                .WithMany(p => p.Invoices_Items)
                .HasForeignKey(ii => ii.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

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

            // 🔹 UnitsLevel relationships
            modelBuilder.Entity<UnitsLevel>()
                .HasOne(a => a.Product)
                .WithMany(b => b.Units)
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade insert & delete

            modelBuilder.Entity<UnitsLevel>()
                .HasOne(a => a.UnitProduct)
                .WithMany(b => b.UnitsLevel)
                .HasForeignKey(a => a.UnitProductId);

            // 🔹 ProductPrices relationships
            modelBuilder.Entity<ProductPrices>()
                .HasOne(a => a.ProductUnit)
                .WithMany(b => b.Prices)
                .HasForeignKey(a => a.UnitLevelId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade insert & delete

            modelBuilder.Entity<ProductPrices>()
                .HasOne(a => a.PriceLevel)
                .WithMany(b => b.ProductPrices)
                .HasForeignKey(a => a.PriceLevelId);

            // 🔹 ProductBarcodes relationships
            modelBuilder.Entity<ProductBarcodes>()
                .HasOne(a => a.ProductUnit)
                .WithMany(b => b.Barcodes)
                .HasForeignKey(a => a.ProductUnitId)
                .OnDelete(DeleteBehavior.Cascade); // مهم برای insert خودکار Barcodes

            // 🔹 Optional: Configure max lengths, required fields
            modelBuilder.Entity<ProductBarcodes>()
                .Property(b => b.Barcode)
                .HasMaxLength(60)
                .IsRequired();

            // 🔹 Optional: Configure decimal precision for prices
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
        // =============================
        // 2️⃣ Override SaveChanges
        // =============================
        public override int SaveChanges()
        {
            SoftDeleteInterceptor();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            SoftDeleteInterceptor();
            return base.SaveChangesAsync(cancellationToken);
        }

        // =============================
        // 3️⃣ Soft Delete Interceptor
        // =============================
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
    }
}

