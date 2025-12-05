using AutoMapper;
using Prime_Software.DTO.Product;
using BusinessEntity.Product;

namespace Prime_Software.DTO
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<DTO.Product.Product, BusinessEntity.Product.Product>();
            CreateMap<DTO.Product.UnitLevel, BusinessEntity.Product.UnitsLevel>();
            CreateMap<DTO.Product.Barcodes, BusinessEntity.Product.ProductBarcodes>();
            CreateMap<DTO.Product.Price, BusinessEntity.Product.ProductPrices>();
        }
    }
}
