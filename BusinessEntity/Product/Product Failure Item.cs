using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Product
{
    public class Product_Failure_Item
    {
        public int Id { get; set; }

        [MaxLength(40)]
        public string Barcode { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(40)]
        public string Unit { get; set; } = string.Empty;

        public int StoreroomId { get; set; }
        public Storeroom_Product? Storeroom_Product { get; set; }

        public int Value { get; set; }

        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        public int ProductFailureId { get; set; }
        public Product_Failure ProductFailure { get; set; } = null!;
    }

}
