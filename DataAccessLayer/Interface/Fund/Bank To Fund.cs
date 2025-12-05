using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Fund
{
    public interface IBankToFundRepository
    {
        Task<List<BusinessEntity.Fund.Bank_To_Fund>> Search(DateTime? DateFirst = null, DateTime? DateEnd = null, long? AmountFirst = null, long? AmountEnd = null, int? Fund = null, int? Bnak = null, string? Sand = null, int? People = null, string? Description = null);
        Task<IEnumerable<BusinessEntity.Fund.Bank_To_Fund>> GetAll();
        Task<BusinessEntity.Fund.Bank_To_Fund?> GetById(int Id);
        Task<string> Create(int UserId, BusinessEntity.Fund.Bank_To_Fund Bank_To_Fund);
        Task<string> Update(int UserId, BusinessEntity.Fund.Bank_To_Fund Bank_To_Fund);
    }
}
