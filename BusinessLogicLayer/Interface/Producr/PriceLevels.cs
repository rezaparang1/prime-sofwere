using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Producr
{
    public interface IPriceLevelsService
    {
        Task<IEnumerable<BusinessEntity.Product.PriceLevels>> GetAll();
        Task<BusinessEntity.Product.PriceLevels?> GetById(int id);
        Task<string> Create(int UserId, BusinessEntity.Product.PriceLevels PriceLevels);
        Task<string> Update(int UserId, BusinessEntity.Product.PriceLevels PriceLevels);
        Task<string> Delete(int UserId, int id);
    }
}
