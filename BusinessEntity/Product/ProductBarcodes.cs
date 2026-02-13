using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey(nameof(ProductUnitId))]
        public UnitsLevel? ProductUnit { get; set; }

        [MaxLength(60)]
        public string Barcode { get; set; } = string.Empty;
    }
}
