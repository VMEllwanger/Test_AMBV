using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Constants;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser
{
    public class AuthenticateUserValidator : AbstractValidator<AuthenticateUserCommand>
    {
        public AuthenticateUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(ValidationMessages.EmailRequired)
                .EmailAddress()
                .WithMessage(ValidationMessages.EmailInvalidFormat);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ValidationMessages.PasswordRequired)
                .MinimumLength(6);
        }
    }
}
