using FluentValidation;
using System.Text.RegularExpressions;
using Ambev.DeveloperEvaluation.Domain.Constants;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(email => email)
            .NotEmpty()
            .WithMessage(ValidationMessages.EmailRequired)
            .MaximumLength(100)
            .WithMessage(ValidationMessages.EmailMaxLength)
            .Must(BeValidEmail)
            .WithMessage(ValidationMessages.EmailInvalidFormat);
    }

    private bool BeValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        // More strict email validation
        var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        return regex.IsMatch(email);
    }
}
