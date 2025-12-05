using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Bank
{
    public interface IPayToBankRepository
    {
        Task<IEnumerable<BusinessEntity.Bank.PayToBankListDto>> Search(DateTime? DateFirst = null, DateTime? DateEnd = null, long? AmountFirst = null, long? AmountEnd = null, int? FundId = null, int? BankId = null, string? Sand = null, int? PeopleId = null, string? Description = null);
        Task<IEnumerable<BusinessEntity.Bank.PayToBankListDto>> GetAll();
        Task<BusinessEntity.Bank.Pay_To_Bank?> GetById(int Id);
        Task<string> Create(int UserId, BusinessEntity.Bank.Pay_To_Bank Pay_To_Bank);
        Task<string> Update(int UserId, BusinessEntity.Bank.Pay_To_Bank Pay_To_Bank);
    }
}
