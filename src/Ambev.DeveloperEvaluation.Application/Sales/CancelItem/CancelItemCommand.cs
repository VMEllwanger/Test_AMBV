using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;
[ExcludeFromCodeCoverage]
public class CancelItemCommand : IRequest<CancelItemResult>
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
}
