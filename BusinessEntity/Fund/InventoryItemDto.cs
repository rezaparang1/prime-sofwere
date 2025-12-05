using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Fund
{
    public class InventoryItemDto
    {
        public string Type { get; set; } = string.Empty; // "Bank" یا "Fund"
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // نام بانک یا نام صندوق
        public string AccountNumber { get; set; } = string.Empty;
        public long Inventory { get; set; }
    }
}
