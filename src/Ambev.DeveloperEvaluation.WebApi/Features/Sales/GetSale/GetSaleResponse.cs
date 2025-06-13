namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public class GetSaleResponse
{
  public Guid Id { get; set; }
  public string Branch { get; set; } = string.Empty;
  public string Customer { get; set; } = string.Empty;
  public List<GetSaleItemResponse> Items { get; set; } = new();
  public decimal TotalAmount { get; set; }
  public DateTime CreatedAt { get; set; }
  public bool IsCancelled { get; set; }
}

public class GetSaleItemResponse
{
  public Guid Id { get; set; }
  public Guid ProductId { get; set; }
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
  public decimal TotalAmount { get; set; }
}
