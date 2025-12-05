using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ValidatData.Settings
{
    public class UserValidator : AbstractValidator<BusinessEntity.Settings.User>
    {
        public UserValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("نام کاربری نمیتواند خالی باشد مجددا تلاش کنید .")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(30).WithMessage("نام  کاربری نباید بیش از 30 کاراکتر باشد.");
            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("رمزعبور نمیتواند خالی باشد مجددا تلاش کنید .")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(30).WithMessage("رمزعبور نباید بیش از 30 کاراکتر باشد.");
        }
    }
}
