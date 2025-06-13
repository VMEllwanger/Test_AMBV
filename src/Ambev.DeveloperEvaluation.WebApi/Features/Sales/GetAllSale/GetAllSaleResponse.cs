namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSale;

public class GetAllSaleResponse
{
  public List<GetAllSaleItemResponse> Items { get; set; } = new();
  public int TotalCount { get; set; }
  public int PageCount { get; set; }
  public int CurrentPage { get; set; }
  public int PageSize { get; set; }
}

public class GetAllSaleItemResponse
{
  public Guid Id { get; set; }
  public string Branch { get; set; } = string.Empty;
  public string Customer { get; set; } = string.Empty;
  public decimal TotalAmount { get; set; }
  public DateTime CreatedAt { get; set; }
  public bool IsCancelled { get; set; }
}
