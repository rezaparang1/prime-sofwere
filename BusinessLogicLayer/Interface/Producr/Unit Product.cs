using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Producr
{
    public interface IUnitProductService
    {
        Task<IEnumerable<BusinessEntity.Product.Unit_Product>> GetAll();
        Task<BusinessEntity.Product.Unit_Product?> GetById(int id);
        Task<string> Create(int UserId, BusinessEntity.Product.Unit_Product Unit_Product);
        Task<string> Update(int UserId, BusinessEntity.Product.Unit_Product Unit_Product);
        Task<string> Delete(int UserId, int id);
    }
}
