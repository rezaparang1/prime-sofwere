using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Product
{
    public class PriceLevels
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public bool IsDelete { get; set; }
        public ICollection<People.People> People { get; set; } = new List<People.People>();
        public ICollection<ProductPrices> ProductPrices { get; set; } = new List<ProductPrices>();
    }
}
