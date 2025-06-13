using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelItemSale;

public class CancelItemSaleRequestValidator : AbstractValidator<CancelItemSaleRequest>
{
  public CancelItemSaleRequestValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("Item ID is required");
  }
}
