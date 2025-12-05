using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ValidatData.People
{
    public class TypePeopleValidator : AbstractValidator<BusinessEntity.People.Type_People>
    {
        public TypePeopleValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("نام نوع اشخاص نمیتواند خالی باشد مجددا تلاش کنید .")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(50).WithMessage("نام  نوع اشخاص نباید بیش از 50 کاراکتر باشد.");
        }
    }
}
