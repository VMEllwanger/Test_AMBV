namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleResult
{
  public Guid Id { get; set; }
  public string SaleNumber { get; set; } = string.Empty;
  public DateTime Date { get; set; }
  public string Customer { get; set; } = string.Empty;
  public string Branch { get; set; } = string.Empty;
  public decimal TotalAmount { get; set; }
  public bool IsCancelled { get; set; }
  public List<UpdateSaleItemResult> Items { get; set; } = new();
}

public class UpdateSaleItemResult
{
  public Guid Id { get; set; }
  public string ProductId { get; set; } = string.Empty;
  public string ProductName { get; set; } = string.Empty;
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
  public decimal Discount { get; set; }
  public decimal TotalAmount { get; set; }
}
