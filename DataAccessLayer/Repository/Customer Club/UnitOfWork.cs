using BusinessEntity.Customer_Club;
using DataAccessLayer.Interface.Customer_Club;
using DataAccessLayer.Interface.Product_and_Peopel;
using DataAccessLayer.Repository.Product_and_Peopel;
using Microsoft.EntityFrameworkCore.Storage;
using BusinessEntity.People;
using BusinessEntity.Product;
using BusinessEntity.Invoices;

namespace DataAccessLayer.Repository.Customer_Club
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Database _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(Database context)
        {
            _context = context;

            // ========== ریپازیتوری‌های باشگاه مشتریان ==========
            Stores = new Repository<Store>(_context);
            Customers = new CustomerRepository(_context);
            CustomerLevels = new Repository<CustomerLevel>(_context);
            CustomerLevelHistories = new Repository<CustomerLevelHistory>(_context);
            Wallets = new WalletRepository(_context);
            WalletTransactions = new Repository<WalletTransaction>(_context);
            ClubDiscounts = new ClubDiscountRepository(_context);
            ClubDiscountProducts = new Repository<ClubDiscountProduct>(_context);
            PublicDiscounts = new PublicDiscountRepository(_context);
            PublicDiscountProducts = new Repository<PublicDiscountProduct>(_context);
            PointTransactions = new Repository<PointTransaction>(_context);

            // ========== ریپازیتوری‌های محصولات و قیمت‌گذاری ==========
            Products = new Repository<BusinessEntity.Product.Product>(_context);
            UnitsLevels = new Repository<UnitsLevel>(_context);
            ProductBarcodes = new ProductBarcodeRepository(_context);
            PriceLevels = new Repository<PriceLevels>(_context);
            ProductPrices = new Repository<ProductPrices>(_context);

            // ========== ریپازیتوری اشخاص ==========
            People = new Repository<People>(_context);

            // ========== ریپازیتوری‌های فاکتور ==========
            Invoices = new Repository<BusinessEntity.Invoices.Invoices>(_context);
            InvoiceItems = new Repository<Invoices_Item>(_context);
        }

        // ========== ریپازیتوری‌های باشگاه مشتریان ==========
        public IRepository<Store> Stores { get; }
        public ICustomerRepository Customers { get; }
        public IRepository<CustomerLevel> CustomerLevels { get; }
        public IRepository<CustomerLevelHistory> CustomerLevelHistories { get; }
        public IWalletRepository Wallets { get; }
        public IRepository<WalletTransaction> WalletTransactions { get; }
        public IClubDiscountRepository ClubDiscounts { get; }
        public IRepository<ClubDiscountProduct> ClubDiscountProducts { get; }
        public IPublicDiscountRepository PublicDiscounts { get; }
        public IRepository<PublicDiscountProduct> PublicDiscountProducts { get; }
        public IRepository<PointTransaction> PointTransactions { get; }

        // ========== ریپازیتوری‌های محصولات و قیمت‌گذاری ==========
        public IRepository<BusinessEntity.Product.Product> Products { get; }
        public IRepository<UnitsLevel> UnitsLevels { get; }
        public IProductBarcodeRepository ProductBarcodes { get; }
        public IRepository<PriceLevels> PriceLevels { get; }
        public IRepository<ProductPrices> ProductPrices { get; }

        // ========== ریپازیتوری اشخاص ==========
        public IRepository<People> People { get; }

        // ========== ریپازیتوری‌های فاکتور ==========
        public IRepository<BusinessEntity.Invoices.Invoices> Invoices { get; }
        public IRepository<Invoices_Item> InvoiceItems { get; }

        // ========== مدیریت تراکنش‌ها ==========
        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
            _transaction?.Dispose();
        }
    }
}
