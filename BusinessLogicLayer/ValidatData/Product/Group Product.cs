using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ValidatData.Product
{
    public class GroupProductValidator : AbstractValidator<BusinessEntity.Product.Group_Product>
    {
        public GroupProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("نام گروه کالا نمیتواند خالی باشد مجددا تلاش کنید .")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(50).WithMessage("نام  گروه کالا نباید بیش از 50 کاراکتر باشد.");
        }
    }
}
