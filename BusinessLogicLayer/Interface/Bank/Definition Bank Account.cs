using BusinessEntity.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface.Bank
{
    public interface IDefinitionBankAccountService
    {
        Task<IEnumerable<BankDetailedStatementDto>> GetBankStatement(int? bankId = null, DateTime? dateFrom = null, DateTime? dateTo = null, string? receiptNumber = null, string? description = null);
        Task<List<BusinessEntity.Bank.Definition_Bank_Account>> Search(string? AccountNumber = null, string? BranchName = null, string? BranchAddres = null, string? TypeAccount = null, string? CardNumber = null, string? BranchId = null, string? BracnhPhone = null, int? BankId = null);
        Task<IEnumerable<BusinessEntity.Bank.Definition_Bank_Account>> GetAll();
        Task<BusinessEntity.Bank.Definition_Bank_Account?> GetById(int Id);
        Task<string> Create(int UserId ,BusinessEntity.Bank.Definition_Bank_Account Definition_Bank);
        Task<string> Update(int UserId ,BusinessEntity.Bank.Definition_Bank_Account Definition_Bank);
      
        Task<string> Delete(int UserId ,int Id);
    }
}
