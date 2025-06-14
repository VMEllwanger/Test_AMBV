using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleValidator()
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
            .SetValidator(new CreateSaleItemValidator());
    }
}

public class CreateSaleItemValidator : AbstractValidator<CreateSaleItemCommand>
{
    public CreateSaleItemValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(ValidationMessages.ProductIdRequired);

        RuleFor(x => x.ProductName)
            .NotEmpty()
            .WithMessage(ValidationMessages.ProductNameRequired);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.QuantityGreaterThanZero)
            .LessThanOrEqualTo(20)
            .WithMessage(ValidationMessages.QuantityLessThanOrEqualTwenty);

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.UnitPriceGreaterThanZero);
    }
}
