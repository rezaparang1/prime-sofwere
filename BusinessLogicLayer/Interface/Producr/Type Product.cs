using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Producr
{
    public interface ITypeProductService
    {
        Task<IEnumerable<BusinessEntity.Product.Type_Product>> GetAll();
        Task<BusinessEntity.Product.Type_Product?> GetById(int id);
        Task<string> Create(int UserId, BusinessEntity.Product.Type_Product Type_Product);
        Task<string> Update(int UserId, BusinessEntity.Product.Type_Product Type_Product);
        Task<string> Delete(int UserId, int id);
    }
}
