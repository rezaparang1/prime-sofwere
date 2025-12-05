using BusinessEntity.Financial_Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessEntity.Bank
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
        public string BranchName {  get; set; } = string.Empty;
        [MaxLength(30)]
        public string BranchId {  get; set; } = string.Empty;
        [MaxLength(30)]
        public string BracnhPhone {  get; set; } = string.Empty;
        [MaxLength(200)]
        public string BranchAddres {  get; set; } = string.Empty;

        public bool CardReader { get; set; }
        public long FirstInventory { get; set; }
        public long Inventory { get; set; }

        public int AccountId { get; set; }
        public Financial_Operations.Account? Account { get; set; } = null!;

        [JsonIgnore]
        public NegativeBalancePolicy NegativeBalancePolicy { get; set; } = NegativeBalancePolicy.No;
    }
}
