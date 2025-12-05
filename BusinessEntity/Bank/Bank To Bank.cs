using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Bank
{
    public class Bank_To_Bank
    {
        public int Id { get; set; }
        public long Amount { get; set; }
        public int PeopleId { get; set; }
        public People.People? People { get; set; } = null!;
        public DateTime Date { get; set; }
        public int BankFirstId { get; set; }
        public Definition_Bank BankFirst { get; set; } = null!;
        public int BankAccountFirstId { get; set; }
        public Bank.Definition_Bank_Account BankAccountFirst { get; set; } = null!;
        [MaxLength(20)]
        public string IdSandFirst { get; set; } = string.Empty;
        public int BankEndId { get; set; }
        public Definition_Bank BankEnd { get; set; } = null!;
        public int BankAccountIdEnd { get; set; }
        public Definition_Bank_Account BankAccountEnd { get; set; } = null!;
        [MaxLength(20)]
        public string IdSandEnd { get; set; } = string.Empty;
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}
