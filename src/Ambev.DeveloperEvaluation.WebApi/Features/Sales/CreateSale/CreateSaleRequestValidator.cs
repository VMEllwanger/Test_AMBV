using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
  public CreateSaleRequestValidator()
  {
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
        .SetValidator(new CreateSaleItemRequestValidator());
  }
}

public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
{
  public CreateSaleItemRequestValidator()
  {
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
