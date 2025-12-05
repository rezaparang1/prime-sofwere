using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ValidatData.Product
{
    public class ProductValidator : AbstractValidator<BusinessEntity.Product.Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.TypeProductId)
                .NotEmpty().WithMessage("نوع کالا نمیتواند خالی باشد مجددا تلاش کنید .");
            RuleFor(p => p.UnitProductId)
                .NotEmpty().WithMessage("واحد کالا نمیتواند خالی باشد مجددا تلاش کنید .");
            RuleFor(p => p.SectionProductId)
                .NotEmpty().WithMessage("بخش کالا نمیتواند خالی باشد مجددا تلاش کنید .");
            RuleFor(p => p.StoreroomProductId)
                .NotEmpty().WithMessage("انبار کالا نمیتواند خالی باشد مجددا تلاش کنید .");
            RuleFor(p => p.GroupProductId)
                .NotEmpty().WithMessage("گروه کالا نمیتواند خالی باشد مجددا تلاش کنید .");
            RuleFor(p => p.BuyPrice)
                .NotEmpty().WithMessage("قیمت خرید کالا نمیتواند خالی باشد مجددا تلاش کنید .");
            RuleFor(p => p.Profit)
                .NotEmpty().WithMessage("سود کالا نمیتواند خالی باشد مجددا تلاش کنید .");
            RuleFor(p => p.SalePrice)
                .NotEmpty().WithMessage("قیمت فروش کالا نمیتواند خالی باشد مجددا تلاش کنید .");
            RuleFor(p => p.Name)
               .NotEmpty().WithMessage("نام کالا نمیتواند خالی باشد مجددا تلاش کنید .")
               .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
               .MaximumLength(200).WithMessage("نام  کالا نباید بیش از 50 کاراکتر باشد.");
            //RuleFor(p => p.Units.bar)
            //   .NotEmpty().WithMessage("نام گروه کالا نمیتواند خالی باشد مجددا تلاش کنید .")
            //   .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
            //   .MaximumLength(50).WithMessage("نام  گروه کالا نباید بیش از 50 کاراکتر باشد.");
        }
    }
}
