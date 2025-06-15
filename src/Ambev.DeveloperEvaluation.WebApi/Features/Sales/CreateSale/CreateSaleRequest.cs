namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequest
{
  public string Branch { get; set; } = string.Empty;
  public string Customer { get; set; } = string.Empty;
  public List<CreateSaleItemRequest> Items { get; set; } = new();
}

public class CreateSaleItemRequest
{
  public Guid ProductId { get; set; }
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
}
