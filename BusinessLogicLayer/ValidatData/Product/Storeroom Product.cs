using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity;

namespace BusinessLogicLayer.ValidatData.Product
{
    public class StoreroomProductValidator : AbstractValidator<BusinessEntity.Product.Storeroom_Product>
    {
        public StoreroomProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("نام صندوق نمیتواند خالی باشد مجددا تلاش کنید .")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(50).WithMessage("نام  صندوق نباید بیش از 50 کاراکتر باشد.");
            RuleFor(p => p.PeopleId)
               .NotEmpty().WithMessage("نام انباردار نمیتواند خالی باشد مجددا تلاش کنید .");
            RuleFor(p => p.SectionProductId)
              .NotEmpty().WithMessage("نام بخش نمیتواند خالی باشد مجددا تلاش کنید .");
            RuleFor(p => p.Description)
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
                .MaximumLength(200).WithMessage(" توضیحات نباید بیش از 200 کاراکتر باشد.");
            RuleFor(p => p.Address)
               .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
               .MaximumLength(200).WithMessage(" آدرس نباید بیش از 200 کاراکتر باشد.");
        }
    }
}
