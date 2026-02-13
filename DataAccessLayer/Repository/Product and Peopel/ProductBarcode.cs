using DataAccessLayer.Interface.Product_and_Peopel;
using DataAccessLayer.Repository.Customer_Club;
using Microsoft.EntityFrameworkCore;
using BusinessEntity.Product;

namespace DataAccessLayer.Repository.Product_and_Peopel
{
    public class ProductBarcodeRepository : Repository<ProductBarcodes>, IProductBarcodeRepository
    {
        public ProductBarcodeRepository(Database context) : base(context) { }

        public async Task<ProductBarcodes?> GetByBarcodeAsync(string barcode)
        {
            return await _dbSet
                .Include(pb => pb.ProductUnit)
                    .ThenInclude(ul => ul.Product)
                .FirstOrDefaultAsync(pb => pb.Barcode == barcode);
        }
    }
}
