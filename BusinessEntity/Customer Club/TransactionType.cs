using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Customer_Club
{
    public enum TransactionType
    {
        Deposit = 1,    // واریز
        Withdraw = 2,   // برداشت
        Refund = 3      // برگشت تخفیف باشگاه
    }
}
