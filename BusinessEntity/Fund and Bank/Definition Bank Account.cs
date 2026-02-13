using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessEntity.Fund
{
    public class Definition_Bank_Account
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public Definition_Bank? Bank { get; set; } = null!;
        [MaxLength(30)]
        public string AccountNumber { get; set; } = string.Empty;
        [MaxLength(30)]
        public string TypeAccount { get; set; } = string.Empty;
        [MaxLength(30)]
        public string PeopleAccount { get; set; } = string.Empty;
        [MaxLength(30)]
        public string CardNumber { get; set; } = string.Empty;
        [MaxLength(30)]
        public string BranchName { get; set; } = string.Empty;
        [MaxLength(30)]
        public string BranchId { get; set; } = string.Empty;
        [MaxLength(30)]
        public string BracnhPhone { get; set; } = string.Empty;
        [MaxLength(200)]
        public string BranchAddres { get; set; } = string.Empty;
        public bool IsDelete { get; set; }
        public bool CardReader { get; set; }
        public long FirstInventory { get; set; }
        public long Inventory { get; set; }
        public int AccountId { get; set; }
        public Invoices.Account? Account { get; set; } = null!;
        [JsonIgnore]
        public NegativeBalancePolicy NegativeBalancePolicy { get; set; } = NegativeBalancePolicy.No;
    }
}
