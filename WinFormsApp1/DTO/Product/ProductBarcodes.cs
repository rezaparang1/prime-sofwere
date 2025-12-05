using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Product
{
    public class ProductBarcodes
    {
        public Guid TempId { get; set; } = Guid.NewGuid();
        public int ProductUnitId { get; set; }
        public string Barcode { get; set; } = string.Empty;
    }
}
