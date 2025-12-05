using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Product
{
    public class ComboItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public override string ToString() => Title;
    }
}
