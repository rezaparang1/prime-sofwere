using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity;

namespace BusinessLogicLayer.ValidatData.Bank
{
    //public class DefinitionBankValidator : AbstractValidator<BusinessEntity.Bank.Definition_Bank>
    //{
    //    public DefinitionBankValidator()
    //    {
    //        RuleFor(p => p.Name)
    //            .NotEmpty().WithMessage("نام بانک نمیتواند خالی باشد مجددا تلاش کنید .")
    //            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("نام نباید فقط فاصله باشد.")
    //            .MaximumLength(20).WithMessage("نام  بانک نباید بیش از 20 کاراکتر باشد.");
    //    }
    //}
}
