using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(x => x.SaleNumber)
            .NotEmpty()
            .WithMessage(ValidationMessages.SaleNumberRequired);

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage(ValidationMessages.SaleDateRequired);

        RuleFor(x => x.Customer)
            .NotEmpty()
            .WithMessage(ValidationMessages.CustomerRequired);

        RuleFor(x => x.Branch)
            .NotEmpty()
            .WithMessage(ValidationMessages.BranchRequired);

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage(ValidationMessages.SaleItemsRequired);

        RuleForEach(x => x.Items)
            .SetValidator(new SaleItemValidator());
    }
}
