using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Financial_Operations
{
    public class Account
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        public AccountType Type { get; set; }
        public Fund.Fund? Fund { get; set; }
        public Bank.Definition_Bank_Account? BankAccount { get; set; }
        public ICollection<Receive_OR_Pay_Item> Items { get; set; } = new List<Receive_OR_Pay_Item>();
    }
}
