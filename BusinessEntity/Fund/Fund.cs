using BusinessEntity.Financial_Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessEntity.Fund
{
    public class Fund
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        public long FirstInventory { get; set; }
        public long Inventory { get; set; }
        public bool IsDelete { get; set; }

        public int AccountId { get; set; }
        public Account? Account { get; set; }

        public NegativeBalancePolicy NegativeBalancePolicy { get; set; } = NegativeBalancePolicy.No;

        public ICollection<Cash_Register_To_The_User> CashRegisters { get; set; }
            = new List<Cash_Register_To_The_User>();
    }
}
