using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class InvoiceCreateDto
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public int PeopleId { get; set; }          // خریدار اصلی (اجباری)
        public int? CustomerId { get; set; }       // عضو باشگاه (اختیاری)
        public int UserId { get; set; }
        public BusinessEntity.Invoices.Type_Pay TypePay { get; set; }
        public int StoreId { get; set; }
        public bool ApplyPublicDiscount { get; set; } = true;
        public bool ApplyClubDiscount { get; set; } = true;
        public bool RefundClubDiscountToWallet { get; set; } = true;
        public int UsedWalletAmount { get; set; } = 0;
        public List<InvoiceItemCreateDto> Items { get; set; } = new();
        public List<PaymentDetailDto> Payments { get; set; } = new();
    }
}
