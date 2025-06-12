using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
{
  public UpdateSaleValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("O ID da venda é obrigatório.");

    RuleFor(x => x.Customer)
        .NotEmpty()
        .WithMessage("O cliente é obrigatório.");

    RuleFor(x => x.Branch)
        .NotEmpty()
        .WithMessage("A filial é obrigatória.");

    RuleFor(x => x.Items)
        .NotEmpty()
        .WithMessage("A venda deve ter pelo menos um item.");

    RuleForEach(x => x.Items).SetValidator(new UpdateSaleItemValidator());
  }
}

public class UpdateSaleItemValidator : AbstractValidator<UpdateSaleItemCommand>
{
  public UpdateSaleItemValidator()
  {
    RuleFor(x => x.ProductId)
        .NotEmpty()
        .WithMessage("O ID do produto é obrigatório.");

    RuleFor(x => x.Quantity)
        .GreaterThan(0)
        .WithMessage("A quantidade deve ser maior que zero.");

    RuleFor(x => x.UnitPrice)
        .GreaterThan(0)
        .WithMessage("O preço unitário deve ser maior que zero.");

    RuleFor(x => x.Discount)
        .GreaterThanOrEqualTo(0)
        .WithMessage("O desconto não pode ser negativo.");
  }
}
