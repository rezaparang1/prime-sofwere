using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Financial_Operations
{
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; } = string.Empty; // مثلاً: بانک ملت، صندوق اصلی
        public string AccountType { get; set; } = string.Empty; // Bank / CashBox / Other
        public decimal Balance { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
