using BusinessEntity.DTO.Bank;
using DataAccessLayer;
using BusinessEntity.Fund;

namespace BusinessLogicLayer.Interface.Fund
{
    public interface IDefinitionBankAccountService
    {
        Task<IEnumerable<BankDetailedStatementDto>> GetBankStatement(int? bankId = null,
            DateTime? dateFrom = null, DateTime? dateTo = null,
            string? receiptNumber = null, string? description = null);

        Task<List<Definition_Bank_Account>> Search(string? accountNumber = null,
            string? branchName = null, string? branchAddres = null,
            string? typeAccount = null, string? cardNumber = null,
            string? branchId = null, string? bracnhPhone = null,
            int? bankId = null);

        Task<IEnumerable<Definition_Bank_Account>> GetAll();
        Task<Definition_Bank_Account?> GetById(int id);
        Task<Result> Create(Definition_Bank_Account bankAccount, int userId);
        Task<Result> Update(Definition_Bank_Account bankAccount, int userId);
        Task<Result> Delete(int id, int userId);
    }
}
