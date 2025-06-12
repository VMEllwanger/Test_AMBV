using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
  public SaleValidator()
  {
    RuleFor(x => x.SaleNumber)
        .NotEmpty()
        .WithMessage("O número da venda é obrigatório");

    RuleFor(x => x.Date)
        .NotEmpty()
        .WithMessage("A data da venda é obrigatória");

    RuleFor(x => x.Customer)
        .NotEmpty()
        .WithMessage("O cliente é obrigatório");

    RuleFor(x => x.Branch)
        .NotEmpty()
        .WithMessage("A filial é obrigatória");

    RuleFor(x => x.Items)
        .NotEmpty()
        .WithMessage("A venda deve ter pelo menos um item");

    RuleForEach(x => x.Items)
        .SetValidator(new SaleItemValidator());
  }
}
