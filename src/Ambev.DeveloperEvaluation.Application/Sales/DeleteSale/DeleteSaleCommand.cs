using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

[ExcludeFromCodeCoverage]
public class DeleteSaleCommand : IRequest<DeleteSaleResult>
{
    public Guid Id { get; set; }
}
