using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ValidatData.People
{
    public class GroupPeopleValidator : AbstractValidator<BusinessEntity.People.Group_People>
    {
        public GroupPeopleValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("نام گروه اشخاص نمیتواند خالی باشد مجددا تلاش کنید .")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(50).WithMessage("نام  گروه اشخاص نباید بیش از 50 کاراکتر باشد.");
        }
    }
}
