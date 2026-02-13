using BusinessEntity.Product;
using BusinessLogicLayer.DTO;
using DataAccessLayer;
using DataAccessLayer.Interface.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLogicLayer.Interface.Producr
{
    public interface IProductService
    {
        Task<IEnumerable<BusinessEntity.DTO.Product.ProductInventoryDto>> GetProductInventoryAsync(string? barcode = null);
        Task<IEnumerable<BusinessEntity.DTO.Product.ProductSalesByDateDto>> GetProductSalesReportByDateAsync(DateTime startDate, DateTime endDate, string? barcode = null);
        //Task<List<BusinessEntity.Product.Product>> SearchbyweightedProducts(bool? Isweighty, bool? IsActive);
        //Task<List<BusinessEntity.Product.Product>> SearchbyButtonProducts();
        //Task<BusinessEntity.Product.Product?> GetProductByBarcode(string? Barcode);
        //Task<BusinessEntity.Product.Product?> GetByShortcutKey(string? ShortcutKey);
        //Task<IEnumerable<BusinessEntity.Product.ProductReportDto>> GetProductReport();
        //Task<BusinessEntity.ProductDto?> GetProductByBarcodeAsync(string Barcode);
        Task<Result<ProductBarcodeInfoDto>> GetProductInfoByBarcodeAsync(string barcode, int? customerId = null);
        Task<IEnumerable<Product>> GetAll();
        Task<Product?> GetById(int id);
        Task<Result> Create(Product product, int userId);
        Task<Result> Update(Product product, int userId);
        Task<Result> Delete(int id, int userId);
        Task<List<Product>> Search(
            string? name = null,
            string? barcode = null,
            int? typeProductId = null,
            bool? isActive = null,
            string? description = null,
            bool? isTax = null,
            int? groupId = null,
            int? storeroomId = null,
            int? unitId = null,
            int? sectionId = null);
        Task<List<Product>> GetProductsForCombo();
    }
}
