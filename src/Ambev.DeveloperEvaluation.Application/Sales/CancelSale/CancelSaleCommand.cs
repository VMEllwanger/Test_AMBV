using MediatR;
using System.Diagnostics.CodeAnalysis;
namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
[ExcludeFromCodeCoverage]
public class CancelSaleCommand : IRequest<CancelSaleResult>
{
    public Guid Id { get; set; }
    public string CancellationReason { get; set; } = string.Empty;
}
