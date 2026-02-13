using BusinessEntity.Settings;
using DataAccessLayer.Interface.Customer_Club;
using BusinessEntity.Product;

namespace DataAccessLayer.Interface.Product_and_Peopel
{
    public interface IProductBarcodeRepository : IRepository<ProductBarcodes>
    {
        Task<ProductBarcodes?> GetByBarcodeAsync(string barcode);
    }
}
