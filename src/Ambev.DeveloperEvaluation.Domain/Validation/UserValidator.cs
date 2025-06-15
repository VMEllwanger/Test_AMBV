using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Email).SetValidator(new EmailValidator());

        RuleFor(user => user.Username)
            .NotEmpty()
            .WithMessage(ValidationMessages.UsernameRequired)
            .MinimumLength(3)
            .WithMessage(ValidationMessages.UsernameMinLength)
            .MaximumLength(50)
            .WithMessage(ValidationMessages.UsernameMaxLength);

        RuleFor(user => user.Password).SetValidator(new PasswordValidator());

        RuleFor(user => user.Phone)
            .Matches(@"^\+[1-9]\d{10,14}$")
            .WithMessage(ValidationMessages.PhoneInvalidFormat);

        RuleFor(user => user.Status)
            .NotEqual(UserStatus.Unknown)
            .WithMessage(ValidationMessages.UserStatusInvalid);

        RuleFor(user => user.Role)
            .NotEqual(UserRole.None)
            .WithMessage(ValidationMessages.UserRoleInvalid);
    }
}
