using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommand : IRequest<CreateSaleResult>
{
  public string SaleNumber { get; set; } = string.Empty;
  public DateTime Date { get; set; }
  public string Customer { get; set; } = string.Empty;
  public string Branch { get; set; } = string.Empty;
  public List<CreateSaleItemCommand> Items { get; set; } = new();
}

public class CreateSaleItemCommand
{
  public string ProductId { get; set; } = string.Empty;
  public string ProductName { get; set; } = string.Empty;
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
}
