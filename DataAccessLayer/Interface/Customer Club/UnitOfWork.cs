using BusinessEntity.Customer_Club;
using DataAccessLayer.Interface.Product_and_Peopel;
using BusinessEntity.Product;
using BusinessEntity.People;

namespace DataAccessLayer.Interface.Customer_Club
{
    public interface IUnitOfWork : IDisposable
    {
        // ========== ریپازیتوری‌های باشگاه مشتریان ==========
        ICustomerRepository Customers { get; }
        IRepository<CustomerLevel> CustomerLevels { get; }
        IRepository<CustomerLevelHistory> CustomerLevelHistories { get; }
        IWalletRepository Wallets { get; }
        IRepository<WalletTransaction> WalletTransactions { get; }
        IClubDiscountRepository ClubDiscounts { get; }
        IRepository<ClubDiscountProduct> ClubDiscountProducts { get; }
        IPublicDiscountRepository PublicDiscounts { get; }
        IRepository<PublicDiscountProduct> PublicDiscountProducts { get; }
        IRepository<PointTransaction> PointTransactions { get; }
        IRepository<Store> Stores { get; }

        // ========== ریپازیتوری‌های محصولات و قیمت‌گذاری ==========
        IRepository<BusinessEntity.Product.Product> Products { get; }        // یکبار تعریف شده
        IRepository<UnitsLevel> UnitsLevels { get; }
        IProductBarcodeRepository ProductBarcodes { get; }
        IRepository<PriceLevels> PriceLevels { get; }
        IRepository<ProductPrices> ProductPrices { get; }

        // ========== ریپازیتوری اشخاص (People) ==========
        IRepository<BusinessEntity.People.People> People { get; }           // با Namespace کامل

        // ========== ریپازیتوری‌های فاکتور ==========
        IRepository<BusinessEntity.Invoices.Invoices> Invoices { get; }
        IRepository<BusinessEntity.Invoices.Invoices_Item> InvoiceItems { get; }

        // ========== مدیریت تراکنش‌ها ==========
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        // ========== ذخیره تغییرات ==========
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
