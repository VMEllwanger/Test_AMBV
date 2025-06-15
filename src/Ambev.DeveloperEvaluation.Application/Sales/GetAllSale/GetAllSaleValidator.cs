using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;

public class GetAllSaleValidator : AbstractValidator<GetAllSaleCommand>
{
    public GetAllSaleValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.PageGreaterThanZero);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.PageSizeGreaterThanZero)
            .LessThanOrEqualTo(100)
            .WithMessage(ValidationMessages.PageSizeLessThanOrEqualHundred);

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage(ValidationMessages.StartDateLessThanEndDate);
    }
}
