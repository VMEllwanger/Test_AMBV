using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;

public class GetAllSaleValidator : AbstractValidator<GetAllSaleCommand>
{
  public GetAllSaleValidator()
  {
    RuleFor(x => x.Page)
        .GreaterThan(0)
        .WithMessage("A página deve ser maior que zero.");

    RuleFor(x => x.PageSize)
        .GreaterThan(0)
        .LessThanOrEqualTo(100)
        .WithMessage("O tamanho da página deve estar entre 1 e 100.");

    RuleFor(x => x.StartDate)
        .LessThanOrEqualTo(x => x.EndDate)
        .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
        .WithMessage("A data inicial deve ser menor ou igual à data final.");
  }
}
