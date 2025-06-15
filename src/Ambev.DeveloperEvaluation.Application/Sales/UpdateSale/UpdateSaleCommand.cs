using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

[ExcludeFromCodeCoverage]
public class UpdateSaleCommand : IRequest<UpdateSaleResult>
{
    public Guid Id { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public List<UpdateSaleItemCommand> Items { get; set; } = new();
}

[ExcludeFromCodeCoverage]
public class UpdateSaleItemCommand
{
    public Guid Id { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
