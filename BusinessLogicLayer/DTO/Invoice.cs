using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int PeopleId { get; set; }
        public string PeopleName { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int TotalOriginalPrice { get; set; }
        public int TotalPublicDiscount { get; set; }
        public int TotalClubDiscount { get; set; }
        public int LevelDiscountAmount { get; set; }
        public int UsedWalletAmount { get; set; }
        public int FinalAmount { get; set; }
        public int EarnedPoints { get; set; }
        public List<InvoiceItemDto> Items { get; set; } = new();
        public List<WalletTransactionDto> WalletTransactions { get; set; } = new();
        public List<PointTransactionDto> PointTransactions { get; set; } = new();
    }
}
