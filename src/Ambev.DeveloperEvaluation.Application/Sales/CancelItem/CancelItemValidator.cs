using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Constants;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

public class CancelItemValidator : AbstractValidator<CancelItemCommand>
{
  public CancelItemValidator()
  {
    RuleFor(x => x.SaleId)
        .NotEmpty()
        .WithMessage(ValidationMessages.SaleIdRequired);

    RuleFor(x => x.ItemId)
        .NotEmpty()
        .WithMessage(ValidationMessages.ItemIdRequired);

  }
}
