using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Invoices
{
    public class Invoices_Item
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product.ProductDto Product { get; set; } = null!;
        public string Barcode { get; set; } = string.Empty;
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        public int Number { get; set; }
        public int Price { get; set; }
        public int OFF { get; set; }
        public int AllPrice { get; set; }
        public int InvoicesId { get; set; }
        public Invoices Invoices { get; set; } = null!;
    }
}
