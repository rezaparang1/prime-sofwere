using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Product
{
    public class Section_Product
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = String.Empty;
        public bool IsDelete { get; set; }
        public ICollection<Storeroom_Product> Storeroom_Product { get; set; } = new List<Storeroom_Product>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
