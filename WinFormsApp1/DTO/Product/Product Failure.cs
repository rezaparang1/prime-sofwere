using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Product
{
    public class Product_Failure
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
        public List<Product_Failure_Item> ProductFailureItem { get; set; } = null!;
    }
}
