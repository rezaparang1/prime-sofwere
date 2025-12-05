using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Product
{
    public class ProductFailureEditDto
    {
        public int FailureId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public int AccountId { get; set; } // حساب مقصد برای ویرایش

        public List<ProductFailureItemEditDto> Items { get; set; } = new();
    }
}
