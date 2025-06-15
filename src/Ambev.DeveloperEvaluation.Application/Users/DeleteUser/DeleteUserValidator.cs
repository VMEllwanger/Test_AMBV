using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Constants;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

/// <summary>
/// Validator for DeleteUserCommand
/// </summary>
public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    /// <summary>
    /// Initializes validation rules for DeleteUserCommand
    /// </summary>
    public DeleteUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(ValidationMessages.UserIdRequired);
    }
}
