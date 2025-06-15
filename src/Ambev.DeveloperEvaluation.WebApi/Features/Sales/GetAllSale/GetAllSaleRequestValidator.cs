using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSale;

public class GetAllSaleRequestValidator : AbstractValidator<GetAllSaleRequest>
{
    public GetAllSaleRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.PageGreaterThanZero);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage(ValidationMessages.PageSizeBetweenOneAndHundred);
    }
}
