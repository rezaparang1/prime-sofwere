using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public class Wallet
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Balance { get; set; } = 0;  // به ریال
        public DateTime LastUpdate { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual ICollection<WalletTransaction>? Transactions { get; set; }
    }
}
