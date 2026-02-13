using System.ComponentModel.DataAnnotations;

namespace BusinessEntity.Fund
{
    public class Fund
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
        public long FirstInventory { get; set; }
        public long Inventory { get; set; }
        public bool IsDelete { get; set; }
        public int AccountId { get; set; }
        public Invoices.Account? Account { get; set; }
        public NegativeBalancePolicy NegativeBalancePolicy { get; set; } = NegativeBalancePolicy.No;
        public ICollection<Cash_Register_To_The_User> CashRegisters { get; set; } = new List<Cash_Register_To_The_User>();
    }
}
