using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ValidatData.Product
{
    public class PriceLevelsValidator : AbstractValidator<BusinessEntity.Product.PriceLevels>
    {
        public PriceLevelsValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("نام سطح قیمت نمیتواند خالی باشد مجددا تلاش کنید .")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(50).WithMessage("نام سطح قیمت نباید بیش از 50 کاراکتر باشد.");
        }
    }
}
