using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.DTO.Product
{
    public class ProductReportDto
    {
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Inventory { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SalePrice { get; set; }
    }
}
