using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class PaymentDetailDto
    {
        public BusinessEntity.Invoices.Type_Pay PaymentType { get; set; }
        public int Amount { get; set; }
        public int? FundId { get; set; }          // برای پرداخت نقدی
        public int? BankAccountId { get; set; }   // برای پرداخت بانکی
    }
}
