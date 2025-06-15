using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

public class CancelSaleRequestValidator : AbstractValidator<CancelSaleRequest>
{
  public CancelSaleRequestValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage(ValidationMessages.SaleIdRequired);

    RuleFor(x => x.CancellationReason)
        .NotEmpty()
        .WithMessage(ValidationMessages.CancellationReasonRequired)
        .MaximumLength(500)
        .WithMessage(ValidationMessages.CancellationReasonMaxLength);
  }
}
