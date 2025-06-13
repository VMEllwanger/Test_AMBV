using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    public UpdateSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O ID da venda é obrigatório");

        RuleFor(x => x.Branch)
            .NotEmpty()
            .WithMessage("A filial é obrigatória");

        RuleFor(x => x.Customer)
            .NotEmpty()
            .WithMessage("O cliente é obrigatório");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("A venda deve ter pelo menos um item");

        RuleForEach(x => x.Items)
            .SetValidator(new UpdateSaleItemRequestValidator());
    }
}

public class UpdateSaleItemRequestValidator : AbstractValidator<UpdateSaleItemRequest>
{
    public UpdateSaleItemRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O ID do item é obrigatório");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("O ID do produto é obrigatório");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior que zero");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("O preço unitário deve ser maior que zero");
    }
}
