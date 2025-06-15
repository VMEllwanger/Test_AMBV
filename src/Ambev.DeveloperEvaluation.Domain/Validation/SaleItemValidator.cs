using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
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
            .WithMessage(ValidationMessages.QuantityMaxLimit);

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.UnitPriceGreaterThanZero);

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ValidationMessages.DiscountCannotBeNegative)
            .LessThanOrEqualTo(1)
            .WithMessage(ValidationMessages.DiscountMaxLimit);
    }
}
