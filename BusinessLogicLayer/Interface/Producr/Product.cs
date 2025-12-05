using BusinessEntity.Product;
using DataAccessLayer.Interface.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.Repository.Product.ProductRepository;

namespace BusinessLogicLayer.Interface.Producr
{
    public interface IProductService
    {
        Task<IEnumerable<ProductInventoryDto>> GetProductInventoryAsync(string? barcode = null);
        Task<IEnumerable<BusinessEntity.Product.ProductSalesByDateDto>> GetProductSalesReportByDateAsync(DateTime startDate, DateTime endDate, string? barcode = null);
        Task<List<BusinessEntity.Product.Product>> SearchbyweightedProducts(bool? Isweighty, bool? IsActive);
        Task<List<BusinessEntity.Product.Product>> SearchbyButtonProducts();
        Task<List<BusinessEntity.Product.Product>> Search(string? Name = null, string? Barcode = null, int? TypeProduct = null, bool? IsActive = null, string? Description = null, bool? IsTax = null, int? GroupId = null, int? StoreroomId = null, int? UnitId = null, int? SectionId = null);
        Task<BusinessEntity.Product.Product?> GetProductByBarcode(string? Barcode);
        Task<BusinessEntity.Product.Product?> GetByShortcutKey(string? ShortcutKey);
        Task<IEnumerable<BusinessEntity.Product.Product>> GetAll();
        Task<IEnumerable<BusinessEntity.Product.ProductReportDto>> GetProductReport();
        Task<BusinessEntity.Product.Product?> GetById(int id);
        Task<BusinessEntity.ProductDto?> GetProductByBarcodeAsync(string Barcode);
        Task<ServiceResult> Create(int UserId, BusinessEntity.Product.Product Product);
        Task<ServiceResult> Update(int UserId, BusinessEntity.Product.Product updatedProduct);
        Task<ServiceResult> Delete(int UserId, int productId);
    }
}
