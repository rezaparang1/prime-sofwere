using BusinessEntity.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interface.Fund
{
    public interface ICashRegisterToTheUserRepository
    {
        Task<List<CashRegisterComboItem>> GetActiveCashRegistersForCombo();
        Task<List<BusinessEntity.Fund.CashRegisterDto>> GetAll();
        Task<BusinessEntity.Fund.Cash_Register_To_The_User?> GetActiveByUser(int userId);
        Task<BusinessEntity.Fund.CashRegisterDto?> GetById(int id);
        Task<string> Create(int UserId ,BusinessEntity.Fund.Cash_Register_To_The_User Cash_Register_To_The_User);
        Task<string> Update(int UserId ,BusinessEntity.Fund.Cash_Register_To_The_User Cash_Register_To_The_User);
    }
}
