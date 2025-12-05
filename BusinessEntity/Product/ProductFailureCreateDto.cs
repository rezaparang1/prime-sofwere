using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Product
{
    public class FailureSubmitModel
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; } = string.Empty;

        // حساب مقصد (صندوق یا حساب بانکی) از ComboBox می‌فرستیم
        public int AccountId { get; set; }

        // اگر انبار سیاست Message داشته باشه و کاربر تایید کنه مقدار true می‌فرستیم
        public bool ForceNegative { get; set; } = false;

        public List<FailureItemModel> Items { get; set; } = new();
    }
}
