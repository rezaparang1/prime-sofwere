using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ValidatData.People
{
    public class PeopleValidator : AbstractValidator<BusinessEntity.People.People>
    {
        public PeopleValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("نام  شخص نمیتواند خالی باشد مجددا تلاش کنید .")
                
                .MaximumLength(50).WithMessage("نام شخص نباید بیش از 50 کاراکتر باشد.");
            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("نام خانوادگی  شخص نمیتواند خالی باشد مجددا تلاش کنید .")
                
                .MaximumLength(50).WithMessage("نام خانوادگی شخص نباید بیش از 50 کاراکتر باشد.");
            RuleFor(p => p.Phone)
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("شماره تماس نباید فقط فاصله باشد.")
                .MaximumLength(12).WithMessage("شماره تماس شخص نباید بیش از 12 کاراکتر باشد.");
            RuleFor(p => p.Description)
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(200).WithMessage("توضیحات شخص نباید بیش از 200 کاراکتر باشد.");
            RuleFor(p => p.Address)
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(200).WithMessage("آدرس شخص نباید بیش از 200 کاراکتر باشد.");
        }
    }
}
