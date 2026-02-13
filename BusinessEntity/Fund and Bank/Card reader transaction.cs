using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Fund
{
    public class Card_reader_transaction
    {
        public int Id { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public int Amount { get; set; }
        public bool Status { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string TrackingNumber { get; set; } = string.Empty;
        public string ForWhath { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string TypeDevice { get; set; } = string.Empty;

    }
}
