using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Constants;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password)
            .NotEmpty()
            .WithMessage(ValidationMessages.PasswordRequired)
            .MinimumLength(8)
            .WithMessage(ValidationMessages.PasswordMinLength)
            .Matches(@"[A-Z]+")
            .WithMessage(ValidationMessages.PasswordUppercaseRequired)
            .Matches(@"[a-z]+")
            .WithMessage(ValidationMessages.PasswordLowercaseRequired)
            .Matches(@"[0-9]+")
            .WithMessage(ValidationMessages.PasswordNumberRequired)
            .Matches(@"[\!\?\*\.\@\#\$\%\^\&\+\=]+")
            .WithMessage(ValidationMessages.PasswordSpecialCharRequired);
    }
}
