using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Financial_Operations
{
    public class Receive_OR_Pay_Item
    {
        public int Id { get; set; }

        public long Amount { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;
        public int ReferenceId { get; set; }
        public FinancialDocumentType ReferenceType { get; set; }
        public int Receive_OR_PayId { get; set; }
        public Receive_OR_Pay Receive_OR_Pay { get; set; } = null!;
    }
}
