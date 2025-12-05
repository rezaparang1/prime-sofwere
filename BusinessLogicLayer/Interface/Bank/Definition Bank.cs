using BusinessEntity.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity;

namespace BusinessLogicLayer.Interface.Bank
{
    public interface IDefinitionBankService
    {
        Task<IEnumerable<BusinessEntity.Bank.Definition_Bank>> GetAll();
        Task<BusinessEntity.Bank.Definition_Bank?> GetById(int Id);
        Task<string> Create(int UserId ,BusinessEntity.Bank.Definition_Bank Definition_Bank);
        Task<string> Update(int UserId ,BusinessEntity.Bank.Definition_Bank Definition_Bank);
        Task<string> Delete(int UserId ,int Id);
    }
}
