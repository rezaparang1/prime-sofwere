using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ValidatData.Fund
{
    public class CashRegisterUserValidator : AbstractValidator<BusinessEntity.Fund.Cash_Register_To_The_User>
    {
        public CashRegisterUserValidator()
        {
            RuleFor(p => p.UserId)
               .NotEmpty().WithMessage("نام کاربر نمیتواند خالی باشد مجددا تلاش کنید .");
            RuleFor(p => p.FundId)
                 .NotEmpty().WithMessage("نام صندوق نمیتواند خالی باشد مجددا تلاش کنید .");
            RuleFor(p => p.Date)
                .NotEmpty().WithMessage("تاریخ نمیتواند خالی باشد مجددا تلاش کنید .");

        }
    }
}
