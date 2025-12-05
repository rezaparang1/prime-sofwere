using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ValidatData.Fund
{
    public class  FundValidator : AbstractValidator<BusinessEntity.Fund.Fund>
    {
        public FundValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("نام صندوق نمیتواند خالی باشد مجددا تلاش کنید .")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(50).WithMessage("نام  صندوق نباید بیش از 50 کاراکتر باشد.");
            RuleFor(p => p.Description)
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(200).WithMessage(" توضیحات نباید بیش از 200 کاراکتر باشد.");
        }
    }
}
