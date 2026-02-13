using BusinessEntity.Product;
using DataAccessLayer.Interface.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BusinessEntity.DTO.Product;

namespace DataAccessLayer.Interface.Product
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductSalesByDateDto>> GetProductSalesReportByDateAsync(
       DateTime startDate,
       DateTime endDate,
       string? barcode = null);
        Task<IEnumerable<ProductInventoryDto>> GetProductInventoryAsync(string? barcode = null);
        //Task<List<BusinessEntity.Product.Product>> SearchbyButtonProducts();

        //Task<BusinessEntity.Product.Product?> GetProductByBarcode(string? Barcode);
        //Task<BusinessEntity.Product.Product?> GetByShortcutKey(string? ShortcutKey);
        //Task<IEnumerable<ProductReportDto>> GetProductReport();     
        Task<List<BusinessEntity.Product.Product>> Search(
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
        Task<IEnumerable<BusinessEntity.Product.Product>> GetAll();
        Task<BusinessEntity.Product.Product?> GetById(int id);
        Task<Result> Create(BusinessEntity.Product.Product product);
        Task<Result> Update(BusinessEntity.Product.Product product);
        Task<Result> Delete(int id);

        // متدهای خاص محصول
        Task<bool> CheckProductExistsInInvoice(int productId);
        Task<List<BusinessEntity.Product.Product>> GetProductsForCombo();
    }
}
