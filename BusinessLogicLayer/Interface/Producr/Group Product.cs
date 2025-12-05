using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Producr
{
    public interface IGroupProductService
    {
        Task<IEnumerable<BusinessEntity.Product.Group_Product>> GetAll();
        Task<BusinessEntity.Product.Group_Product?> GetById(int id);
      
        Task<ServiceResult> Create(int UserId , BusinessEntity.Product.Group_Product Group_Product);
        Task<ServiceResult> Update(int UserId, BusinessEntity.Product.Group_Product Group_Product);
        Task<ServiceResult> Delete(int UserId, int id);
    }
}
