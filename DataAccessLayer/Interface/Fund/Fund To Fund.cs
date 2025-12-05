using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Fund
{
    public interface IFundToFundRepository
    {
        Task<List<BusinessEntity.Fund.Fund_To_Fund>> Search(DateTime? DateFirst = null, DateTime? DateEnd = null, long? AmountFirst = null, long? AmountEnd = null, int? FundFirst = null, int? FundEnd = null, string? Description = null);
        Task<IEnumerable<BusinessEntity.Fund.Fund_To_Fund>> GetAll();
        Task<BusinessEntity.Fund.Fund_To_Fund?> GetById(int Id);
        Task<string> Create(int UserId, BusinessEntity.Fund.Fund_To_Fund Fund_To_Fund);
        Task<string> Update(int UserId, BusinessEntity.Fund.Fund_To_Fund Fund_To_Fund);
    }
}
