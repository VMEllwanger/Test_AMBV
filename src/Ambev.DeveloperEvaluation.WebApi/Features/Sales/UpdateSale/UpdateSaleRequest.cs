namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleRequest
{
  public Guid Id { get; set; }
  public string Branch { get; set; } = string.Empty;
  public string Customer { get; set; } = string.Empty;
  public List<UpdateSaleItemRequest> Items { get; set; } = new();
}

public class UpdateSaleItemRequest
{
  public Guid Id { get; set; }
  public Guid ProductId { get; set; }
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
}
