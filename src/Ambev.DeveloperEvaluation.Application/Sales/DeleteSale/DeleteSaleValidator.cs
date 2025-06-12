using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

public class DeleteSaleValidator : AbstractValidator<DeleteSaleCommand>
{
  public DeleteSaleValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("O ID da venda é obrigatório");
  }
}
