using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleValidator : AbstractValidator<CancelSaleCommand>
{
  public CancelSaleValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("O ID da venda é obrigatório");

    RuleFor(x => x.CancellationReason)
        .NotEmpty()
        .WithMessage("O motivo do cancelamento é obrigatório")
        .MaximumLength(500)
        .WithMessage("O motivo do cancelamento deve ter no máximo 500 caracteres");
  }
}
