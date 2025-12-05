using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class ProductBarcodeDtoForApi
    {
        public string Barcode { get; set; } = string.Empty;
        public int ProductUnitId { get; set; }
    }
}
