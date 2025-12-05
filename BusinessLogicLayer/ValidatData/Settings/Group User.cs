using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ValidatData.Settings
{
    public class GroupUserValidator : AbstractValidator<BusinessEntity.Settings.Group_User>
    {
        public GroupUserValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("نام گروه کاربری نمیتواند خالی باشد مجددا تلاش کنید .")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(50).WithMessage("نام گروه کاربری نباید بیش از 50 کاراکتر باشد.");
        }
    }
}
