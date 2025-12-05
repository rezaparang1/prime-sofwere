using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Bank
{
    public class BankDetailedStatementDto
    {
        public DateTime Date { get; set; }
        public string PersonName { get; set; } = "نامشخص";
        public string Description { get; set; } = "";
        public string OperationType { get; set; } = "";
        public string ReceiptNumber { get; set; } = "-";
        public long Amount { get; set; }
        public long Balance { get; set; }
        public string BankName { get; set; } = "-";
        public string AccountNumber { get; set; } = "-";
    }
}
