using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Bank
{
    public class Pay_To_Bank
    {
        public int Id { get; set; }
        public long Amount { get; set; }
        [MaxLength(20)]
        public string IdSand { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int FundId { get; set; }
        public Fund.Fund? Fund { get; set; } = null!;
        public int PeopleId { get; set; }
        public People.People? People { get; set; } = null!;
        public int BankId { get; set; }
        public Definition_Bank? Bank { get; set; } = null!;
        public int BankAccountId { get; set; }
        public Definition_Bank_Account? BankAccunt { get; set; } = null!;
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}
