using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;

public class DeleteSaleRequestValidator : AbstractValidator<DeleteSaleRequest>
{
  public DeleteSaleRequestValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage(ValidationMessages.SaleIdRequired);
  }
}
