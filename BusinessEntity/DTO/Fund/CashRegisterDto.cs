using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.DTO.Fund
{
    public class CashRegisterDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int FundId { get; set; }
        public string FundName { get; set; } = string.Empty;
        public long InitialAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
    }
}
