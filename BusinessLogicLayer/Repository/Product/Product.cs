using BusinessEntity.Product;
using BusinessLogicLayer.Interface.Producr;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace BusinessLogicLayer.Repository.Product
{
    public class ProductService : Interface.Producr.IProductService
    {
        private readonly DataAccessLayer.Interface.Product.IProductRepository _ProductRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(DataAccessLayer.Interface.Product.IProductRepository ProductRepository, ILogger<ProductService> logger)
        {
            _ProductRepository = ProductRepository;
            _logger = logger;
        }
        //*******SEARCH*******
        public async Task<IEnumerable<BusinessEntity.Product.ProductSalesByDateDto>> GetProductSalesReportByDateAsync(DateTime startDate, DateTime endDate , string? barcode = null)
        {
            _logger.LogInformation("Request to receive Product with ID: {startDate}{endDate}", startDate, endDate);
            var entity = await _ProductRepository.GetProductSalesReportByDateAsync(startDate,endDate , barcode);
            if (entity == null)
                _logger.LogWarning("Product with ID {startDate},{endDate} not found", startDate, endDate);
            else
                _logger.LogInformation("Product with ID {startDate},{endDate} was successfully found", startDate, endDate);

            return entity;
        }

        public async Task<IEnumerable<ProductInventoryDto>> GetProductInventoryAsync(string? barcode = null)
        {
            _logger.LogInformation("Request to receive Product GetProductInventoryAsync ");
            var entity = await _ProductRepository.GetProductInventoryAsync(barcode);
            if (entity == null)
                _logger.LogWarning("Product with ID {barcode} not found",barcode);
            else
                _logger.LogInformation("Product with ID {barcode} was successfully found", barcode);

            return entity;
        }

        public async Task<BusinessEntity.ProductDto?> GetProductByBarcodeAsync(string barcode)
        {
            _logger.LogInformation("Request to receive Product with ID: {Id}", barcode);
            var entity = await _ProductRepository.GetProductByBarcodeAsync(barcode);
            if (entity == null)
                _logger.LogWarning("Product with ID {barcode} not found", barcode);
            else
                _logger.LogInformation("Product with ID {barcode} was successfully found", barcode);

            return entity;
        }
        public async Task<List<BusinessEntity.Product.Product>> Search(string? Name = null, string? Barcode = null, int? TypeProduct = null, bool? IsActive = null, string? Description = null, bool? IsTax = null, int? GroupId = null, int? StoreroomId = null, int? UnitId = null, int? SectionId = null)
        {
            _logger.LogInformation("Request Group_Product search with Async: {Name}{Barcode}{TypeProduct}{IsActive}{Description}{IsTax}{GroupId}{StoreroomId}{UnitId}{SectionId}",
                Name, Barcode, TypeProduct, IsActive, Description, IsTax, GroupId, StoreroomId, UnitId, SectionId);
            var result = await _ProductRepository.Search(Name,Barcode,TypeProduct,IsActive,Description,IsTax,GroupId,StoreroomId,UnitId,SectionId);
            _logger.LogInformation("{Count} results found", result.Count);
            return result;
        }
        public async Task<List<BusinessEntity.Product.Product>> SearchbyweightedProducts(bool? Isweighty, bool? IsActive)
        {
            _logger.LogInformation("Request Group_Product search with Async: {Isweighty}{IsActive}",
               IsActive, Isweighty);
            var result = await _ProductRepository.SearchbyweightedProducts( IsActive, Isweighty);
            _logger.LogInformation("{Count} results found", result.Count);
            return result;
        }
        public async Task<List<BusinessEntity.Product.Product>> SearchbyButtonProducts()
        {
            _logger.LogInformation("Request Group_Product search with Async: Isweighty and IsActive");
            var result = await _ProductRepository.SearchbyButtonProducts();
            _logger.LogInformation("{Count} results found", result.Count);
            return result;
        }
        public async Task<BusinessEntity.Product.Product?> GetProductByBarcode(string? Barcode)
        {
            _logger.LogInformation("Request to receive Product with ID: {Id}", Barcode);
            var entity = await _ProductRepository.GetProductByBarcode(Barcode);
            if (entity == null)
                _logger.LogWarning("Product with ID {Barcode} not found", Barcode);
            else
                _logger.LogInformation("Product with ID {Barcode} was successfully found", Barcode);

            return entity;
        }
        public async Task<BusinessEntity.Product.Product?> GetByShortcutKey(string? ShortcutKey)
        {
            _logger.LogInformation("Request to receive Product with ID: {Id}", ShortcutKey);
            var entity = await _ProductRepository.GetByShortcutKey(ShortcutKey);
            if (entity == null)
                _logger.LogWarning("Product with ID {ShortcutKey} not found", ShortcutKey);
            else
                _logger.LogInformation("Product with ID {ShortcutKey} was successfully found", ShortcutKey);

            return entity;
        }
        //*******READ*********
        public async Task<IEnumerable<BusinessEntity.Product.Product>> GetAll()
        {
            _logger.LogInformation("Request to receive all Group_Product");
            var result = await _ProductRepository.GetAll();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<IEnumerable<BusinessEntity.Product.ProductReportDto>> GetProductReport()
        {
            _logger.LogInformation("Request to receive all Group_Product");
            var result = await _ProductRepository.GetProductReport();
            _logger.LogInformation("{Count} items received", result.Count());
            return result;
        }
        public async Task<BusinessEntity.Product.Product?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Group_Product with ID: {Id}", id);
            var entity = await _ProductRepository.GetById(id);
            if (entity == null)
                _logger.LogWarning("Group_Product with ID {Id} not found", id);
            else
                _logger.LogInformation("Group_Product with ID {Id} was successfully found", id);

            return entity;
        }
        //*****CRUD**********     
        public async Task<ServiceResult> Create(int userId, BusinessEntity.Product.Product product)
        {
            _logger.LogInformation("Request to add new Product: {@Product}", product);

            var repoResult = await _ProductRepository.Create(userId, product);

            var result = new ServiceResult(repoResult.Success, repoResult.Message);

            _logger.LogInformation("Add result: {@Result}", result);
            return result;
        }
        public async Task<ServiceResult> Update(int userId, BusinessEntity.Product.Product product)
        {
            _logger.LogInformation("Request to update Product: {@Product}", product);

            var existing = await _ProductRepository.GetById(product.Id);
            if (existing == null)
            {
                _logger.LogWarning("Product with ID: {Id} not found for update.", product.Id);
                return new ServiceResult(false, "کالا پیدا نشد.");
            }

            var repoResult = await _ProductRepository.Update(userId, product);
            var result = new ServiceResult(repoResult.Success, repoResult.Message);

            _logger.LogInformation("Update result: {@Result}", result);
            return result;
        }
        public async Task<ServiceResult> Delete(int userId, int productId)
        {
            _logger.LogInformation("Request to delete Product with ID: {Id}", productId);

            var repoResult = await _ProductRepository.Delete(userId, productId);
            var result = new ServiceResult(repoResult.Success, repoResult.Message);

            _logger.LogInformation("Delete result: {@Result}", result);
            return result;
        }
    }
}
