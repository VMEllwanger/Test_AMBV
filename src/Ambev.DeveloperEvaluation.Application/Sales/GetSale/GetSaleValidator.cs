using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Constants;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleValidator : AbstractValidator<GetSaleCommand>
{
  public GetSaleValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage(ValidationMessages.SaleIdRequired);
  }
}
