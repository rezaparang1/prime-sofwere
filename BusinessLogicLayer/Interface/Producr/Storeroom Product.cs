using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Producr
{
    public interface IStoreroomProductService
    {
        Task<List<BusinessEntity.Product.StoreroomProductDto>> Search(string? Name = null, string? Description = null, int? IdSection = null);
        Task<IEnumerable<BusinessEntity.Product.StoreroomProductDto>> GetAll();
        Task<BusinessEntity.Product.StoreroomProductDto?> GetById(int id);
        Task<string> Create(int UserId, BusinessEntity.Product.Storeroom_Product Storeroom_Product);
        Task<string> Update(int UserId, BusinessEntity.Product.Storeroom_Product Storeroom_Product);
        Task<string> Delete(int UserId, int id);
    }
}
