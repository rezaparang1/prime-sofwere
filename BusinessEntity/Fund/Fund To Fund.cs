using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Fund
{
    public class Fund_To_Fund
    {
        public int Id { get; set; }

        public int FirstFundId { get; set; }
        public Fund? FirstFund { get; set; } = null!;

        public long InventoryFirst { get; set; }

        public int EndFundId { get; set; }
        public Fund? EndFund { get; set; } = null!;

        public long InventoryEen { get; set; }
        public long Price { get; set; }
        [MaxLength(200)]
        public string Description { get; set; } = String.Empty;
        public DateTime Date { get; set; }
    }
}
