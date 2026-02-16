using BusinessEntity.Product;
using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.Interface.Producr
{
    public interface IProductService
    {
        Task<Result<ProductBarcodeInfoDto>> GetProductInfoForInvoiceAsync(string barcode,int? peopleId = null,int? customerId = null,int? storeId = null);
        Task<IEnumerable<BusinessEntity.DTO.Product.ProductInventoryDto>> GetProductInventoryAsync(string? barcode = null);
        Task<IEnumerable<BusinessEntity.DTO.Product.ProductSalesByDateDto>> GetProductSalesReportByDateAsync(DateTime startDate, DateTime endDate, string? barcode = null);
        Task<List<BusinessEntity.Product.Product>> GetActiveButtonProductsAsync();
        Task<List<BusinessEntity.Product.Product>> GetActiveWeightyProductsAsync();
        Task<List<BusinessEntity.Product.Product>> GetActiveBarcodeProductsAsync();
        Task<List<BusinessEntity.Product.Product>> GetActiveProductsWithShortcutKeyAsync();
        Task<Result<ProductBarcodeInfoDto>> GetProductInfoByBarcodeAsync(string barcode, int? customerId = null, int? storeId = null);
        Task<IEnumerable<BusinessEntity.Product.Product>> GetAll();
        Task<BusinessEntity.Product.Product?> GetById(int id);
        Task<Result> Create(BusinessEntity.Product.Product product, int userId);
        Task<Result> Update(BusinessEntity.Product.Product product, int userId);
        Task<Result> Delete(int id, int userId); 
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
        Task<List<BusinessEntity.Product.Product>> GetProductsForCombo();
    }
}
