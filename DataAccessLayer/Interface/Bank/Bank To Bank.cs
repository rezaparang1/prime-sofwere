using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Bank
{
    public interface IBankToBankRepository
    {
        Task<IEnumerable<BusinessEntity.Bank.BankToBankListDto>> Search(DateTime? DateFirst = null, DateTime ? DateEnd = null, long? AmountFirst = null, long? AmountEnd = null , int ? BankFirst = null , int ? BankEnd = null , string? SandFirst = null , string? SandEnd = null ,string? Description = null);
        Task<IEnumerable<BusinessEntity.Bank.BankToBankListDto>> GetAll();
        Task<BusinessEntity.Bank.BankToBankListDto?> GetById(int Id);
        Task<string> Create(int UserId, BusinessEntity.Bank.Bank_To_Bank Bank_To_Bank);
        Task<string> Update(int UserId, BusinessEntity.Bank.Bank_To_Bank Bank_To_Bank);
    }
}
