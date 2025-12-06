using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Product
{
    public class OFF_Product
    {
        public int Id { get; set; }
        public DateTime DateOfSave { get; set; }
        public int GroupUserId { get; set; }
        public Settings.Group_User Group_User { get; set; } = null!;
        public int PriceLevelId { get; set; }
        public PriceLevels PriceLevels { get; set; } = null!;
        public DateTime StartOff {  get; set; }
        public DateTime EndOff { get; set; }
        public TimeSpan StartHours { get; set; }
        public TimeSpan EndHours { get; set; }
    }
}
