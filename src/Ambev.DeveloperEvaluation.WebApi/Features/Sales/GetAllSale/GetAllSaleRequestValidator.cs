using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSale;

public class GetAllSaleRequestValidator : AbstractValidator<GetAllSaleRequest>
{
  public GetAllSaleRequestValidator()
  {
    RuleFor(x => x.Page)
        .GreaterThan(0)
        .WithMessage("A página deve ser maior que zero");

    RuleFor(x => x.PageSize)
        .GreaterThan(0)
        .LessThanOrEqualTo(100)
        .WithMessage("O tamanho da página deve estar entre 1 e 100");
  }
}
