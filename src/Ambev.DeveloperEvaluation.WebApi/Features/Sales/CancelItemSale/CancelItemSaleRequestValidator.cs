using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelItemSale;

public class CancelItemSaleRequestValidator : AbstractValidator<CancelItemSaleRequest>
{
  public CancelItemSaleRequestValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage(ValidationMessages.ItemIdRequired);
  }
}
