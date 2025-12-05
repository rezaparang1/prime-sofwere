using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Producr
{
    public interface ISectionProductService
    {
        Task<IEnumerable<BusinessEntity.Product.Section_Product>> GetAll();
        Task<BusinessEntity.Product.Section_Product?> GetById(int id);
        Task<string> Create(int UserId, BusinessEntity.Product.Section_Product Section_Product);
        Task<string> Update(int UserId, BusinessEntity.Product.Section_Product Section_Product);
        Task<string> Delete(int UserId, int id);
    }
}
