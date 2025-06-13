using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

public class CancelItemValidator : AbstractValidator<CancelItemCommand>
{
  public CancelItemValidator()
  {
    RuleFor(x => x.SaleId)
        .NotEmpty()
        .WithMessage("O ID da venda é obrigatório");

    RuleFor(x => x.ItemId)
        .NotEmpty()
        .WithMessage("O ID do item é obrigatório");

  }
}
