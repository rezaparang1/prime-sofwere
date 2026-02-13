using BusinessEntity.Customer_Club;
using DataAccessLayer.Interface.Customer_Club;
using DataAccessLayer.Interface.Product_and_Peopel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        // ========== باشگاه مشتریان ==========
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

        IRepository<BusinessEntity.Fund.Fund> Funds { get; }
        IRepository<BusinessEntity.Fund.Definition_Bank_Account> BankAccounts { get; }
        IRepository<BusinessEntity.Invoices.Transaction> Transactions { get; }

        // ========== محصولات و قیمت‌گذاری ==========
        IRepository<BusinessEntity.Product.Product> Products { get; }
        IRepository<BusinessEntity.Product.UnitsLevel> UnitsLevels { get; }
        IProductBarcodeRepository ProductBarcodes { get; }   // ✅ از Namespace اصلاح‌شده
        IRepository<BusinessEntity.Product.PriceLevels> PriceLevels { get; }
        IRepository<BusinessEntity.Product.ProductPrices> ProductPrices { get; }
        IRepository<BusinessEntity.Product.Storeroom_Product> StoreroomProducts { get; }

        // ========== اشخاص ==========
        IRepository<BusinessEntity.People.People> People { get; }

        // ========== فاکتور ==========
        IRepository<BusinessEntity.Invoices.Invoices> Invoices { get; }
        IRepository<BusinessEntity.Invoices.Invoices_Item> InvoiceItems { get; }

        // ========== تراکنش ==========
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
