using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Settings
{
    public class Access_Level
    {
        public int Id { get; set; }
        public int GroupUserId { get; set; }
        public Settings.Group_User? Group_User { get; set; } = null!;


        public bool IsSalesInvoice { get; set; }//فاکتور فروش
        public bool IsSalesInvoiceR { get; set; }
        public bool IsPurchaseInvoice { get; set; }//فاکتور خرید 
        public bool IsPurchaseInvoiceR { get; set; }
        public bool IsPurchaseRInvoice { get; set; }//فاکتور برگشت از خرید 
        public bool IsPurchaseRInvoiceR { get; set; }
        public bool IsSalesRInvoice { get; set; }//فاکتور برگشت از فروش
        public bool IsSalesRInvoiceR { get; set; }
        public bool IsOerder { get; set; }
        public bool IsOerderR { get; set; }
        public bool IsDefinitionProduct { get; set; }
        public bool IsKardexProductR { get; set; }
        public bool IsPublicDiscount { get; set; }
        public bool IsStoreroomProduct { get; set; }
        public bool IsProductFailure { get; set; }
        public bool IsProductFailureR { get; set; }
        public bool IsInventoryProduct{ get; set; }
        public bool IsOrderPointR { get; set; }
        public bool IsMonthlyProductR { get; set; }
        public bool IsQuarterlyProductR { get; set; }
        public bool IsViewingofOthers { get; set; }
        public bool IsDefinitionPeople { get; set; }
        public bool IsSMSDefinition { get; set; }
        public bool IsSendSMS { get; set; }
        public bool IsPeopleR { get; set; }
        public bool IsSentMessagesR { get; set; }
        public bool IsSMSLogR { get; set; }
        public bool IsBankAccountDefinition { get; set; }
        public bool IsFund { get; set; }
        public bool IsDeliverytoUser { get; set; }   
        public bool IsShiftWork { get; set; }
        public bool IsBankandFundInventory { get; set; }
        public bool IsBasicData { get; set; }
        public bool IsUser { get; set; }
        public bool IsGroupUser { get; set; }
        public bool IsBackup { get; set; }
        public bool IsRecovery { get; set; }
        public bool IsAccessLevelDefinition { get; set; }
        public bool IsSystemlog { get; set; }
        public bool IsDailySalesR { get; set; }
        public bool IsBankDetailedR { get; set; }
        public bool IsFundDetailedR { get; set; }
        public bool IsCardTransactionR { get; set; }
        public bool IsDetailedProductDiscountR { get; set; }
        public bool IsSalesPeopleR { get; set; }//گزارش فروش به  اشخاص
        public bool IsPurchasePeopleR { get; set; }//گزارش خرید از اشخاص
        public bool IsProductTransactionR { get; set; }
        public bool IsCustomerDefinition{ get; set; }
        public bool IsCustomerReport { get; set; }
        public bool IsCustomerLevelDefinition { get; set; }
        public bool IsCustomerLevelReport { get; set; }
        public bool IsWalletIncreaseandDecrease { get; set; }
        public bool IsClubDiscountDefinition { get; set; }
        public bool IsClubDiscountReport { get; set; }
        public bool IsCustomerDetailR { get; set; }
        public bool IsDelete { get; set; }
        public ICollection<Group_User> Groups { get; set; } = new List<Group_User>();

    }
}
