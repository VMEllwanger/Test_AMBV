using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

[ExcludeFromCodeCoverage]
public class GetSaleCommand : IRequest<GetSaleResult>
{
    public Guid Id { get; set; }
}
