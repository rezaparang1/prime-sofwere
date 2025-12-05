using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Producr
{
    public interface IProductFailureService
    {
        Task<List<BusinessEntity.Product.Product_Failure>> Search(string? name = null);
        Task<IEnumerable<BusinessEntity.Product.Product_Failure>> GetAll();
        Task<BusinessEntity.Product.Product_Failure?> GetById(int id);
        Task<string> Create(BusinessEntity.Product.Product_Failure Product_Failure);
        Task<string> Update(BusinessEntity.Product.Product_Failure Product_Failure);
        Task<string> Delete(int id);
    }
}
