using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    public UpdateSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(ValidationMessages.SaleIdRequired);

        RuleFor(x => x.Branch)
            .NotEmpty()
            .WithMessage(ValidationMessages.BranchRequired);

        RuleFor(x => x.Customer)
            .NotEmpty()
            .WithMessage(ValidationMessages.CustomerRequired);

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage(ValidationMessages.SaleItemsRequired);

        RuleForEach(x => x.Items)
            .SetValidator(new UpdateSaleItemRequestValidator());
    }
}

public class UpdateSaleItemRequestValidator : AbstractValidator<UpdateSaleItemRequest>
{
    public UpdateSaleItemRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(ValidationMessages.ItemIdRequired);

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(ValidationMessages.ProductIdRequired);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.QuantityGreaterThanZero);

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.UnitPriceGreaterThanZero);
    }
}
