using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.DTO.Fund
{
    public class InventoryItemDto
    {
        public string Type { get; set; } = string.Empty; 
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string AccountNumber { get; set; } = string.Empty;
        public long Inventory { get; set; }
    }
}
