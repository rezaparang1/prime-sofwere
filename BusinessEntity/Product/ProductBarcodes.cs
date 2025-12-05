using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessEntity.Product
{
    public class ProductBarcodes
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int ProductUnitId { get; set; }
        public UnitsLevel? ProductUnit { get; set; } = null!;

        [MaxLength(60)]
        public string Barcode { get; set; } = string.Empty;
    }
}
