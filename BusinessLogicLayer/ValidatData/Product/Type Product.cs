using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ValidatData.Product
{
    public class TypeProductValidator : AbstractValidator<BusinessEntity.Product.Type_Product>
    {
        public TypeProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("نام نوع کالا نمیتواند خالی باشد مجددا تلاش کنید .")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(50).WithMessage("نام نوع کالا نباید بیش از 50 کاراکتر باشد.");
        }
    }
}
