using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Fund
{
    public class Bank_To_Fund
    {
        public int Id { get; set; }
        public long Price { get; set; }
        public DateTime Date { get; set; }

        public int BankId { get; set; }
        public Bank.Definition_Bank? Bank { get; set; } = null!;

        public int BankAccountId { get; set; }
        public Bank.Definition_Bank_Account? BankAccount { get; set; } = null!;
        public long InventoryBank { get; set; }
        [MaxLength(20)]
        public string IdSand { get; set; } = string.Empty;

        public int FundId { get; set; }
        public Fund? Fund { get; set; } = null!;
        public long InventoryFund { get; set; }

        public int PeopleId { get; set; }
        public People.People? People { get; set; } = null!;
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}
