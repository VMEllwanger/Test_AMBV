using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Constants;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class PhoneValidator : AbstractValidator<string>
{
    public PhoneValidator()
    {
        RuleFor(phone => phone)
            .NotEmpty()
            .WithMessage(ValidationMessages.PhoneRequired)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage(ValidationMessages.PhoneInvalidFormat);
    }
}
