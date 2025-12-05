using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.DTO.Fund
{
    public class Fund
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public long FirstInventory { get; set; }
        public bool IsDelete { get; set; }
        public NegativeBalancePolicy NegativeBalancePolicy { get; set; } = NegativeBalancePolicy.No;
    }
}
