using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Product
{
    public class OFF_Product_Items
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        [MaxLength(60)]
        public string Barcode { get; set; } = string.Empty;
        [MaxLength(200)]
        public string Name {  get; set; } = string.Empty;

        public int ProductOffId { get; set; }
        public OFF_Product OFF_Product { get; set; } = null!;
    }
}
