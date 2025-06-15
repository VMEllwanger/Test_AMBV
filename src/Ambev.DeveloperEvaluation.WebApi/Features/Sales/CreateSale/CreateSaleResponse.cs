namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleResponse
{
  public Guid Id { get; set; }
  public string Branch { get; set; } = string.Empty;
  public string Customer { get; set; } = string.Empty;
  public List<CreateSaleItemResponse> Items { get; set; } = new();
  public decimal TotalAmount { get; set; }
  public DateTime CreatedAt { get; set; }
}

public class CreateSaleItemResponse
{
  public Guid Id { get; set; }
  public Guid ProductId { get; set; }
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
  public decimal TotalAmount { get; set; }
}
