using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.DTO.Product
{
    public class ProductFailureSearchFilter
    {
        public List<int>? StoreroomIds { get; set; }       // لیست انبارها
        public List<int>? ProductIds { get; set; }         // لیست کالاها
        public DateTime? FromDate { get; set; }            // تاریخ شروع
        public DateTime? ToDate { get; set; }              // تاریخ پایان
    }
}
