using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleValidator()
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
            .SetValidator(new CreateSaleItemValidator());
    }
}

public class CreateSaleItemValidator : AbstractValidator<CreateSaleItemCommand>
{
    public CreateSaleItemValidator()
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
    }
}
