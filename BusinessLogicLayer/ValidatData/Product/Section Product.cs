using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ValidatData.Product
{
    public class SectionProductValidator : AbstractValidator<BusinessEntity.Product.Section_Product>
    {
        public SectionProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("نام بخش کالا نمیتواند خالی باشد مجددا تلاش کنید .")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(50).WithMessage("نام بخش کالا نباید بیش از 50 کاراکتر باشد.");
        }
    }
}
