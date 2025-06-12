using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleCommand : IRequest<UpdateSaleResult>
{
  public Guid Id { get; set; }
  public string Customer { get; set; } = string.Empty;
  public string Branch { get; set; } = string.Empty;
  public List<UpdateSaleItemCommand> Items { get; set; } = new();
}

public class UpdateSaleItemCommand
{
  public Guid Id { get; set; }
  public string ProductId { get; set; } = string.Empty;
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
  public decimal Discount { get; set; }
}
