using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleItemValidator : AbstractValidator<SaleItem>
{
  public SaleItemValidator()
  {
    RuleFor(x => x.ProductId)
        .NotEmpty()
        .WithMessage("O ID do produto é obrigatório");

    RuleFor(x => x.ProductName)
        .NotEmpty()
        .WithMessage("O nome do produto é obrigatório");

    RuleFor(x => x.Quantity)
        .GreaterThan(0)
        .WithMessage("A quantidade deve ser maior que zero")
        .LessThanOrEqualTo(20)
        .WithMessage("A quantidade não pode ser maior que 20");

    RuleFor(x => x.UnitPrice)
        .GreaterThan(0)
        .WithMessage("O preço unitário deve ser maior que zero");

    RuleFor(x => x.Discount)
        .GreaterThanOrEqualTo(0)
        .WithMessage("O desconto não pode ser negativo")
        .LessThanOrEqualTo(1)
        .WithMessage("O desconto não pode ser maior que 100%");
  }
}
