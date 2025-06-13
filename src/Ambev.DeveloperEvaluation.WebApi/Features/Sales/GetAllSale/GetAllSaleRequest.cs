namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSale;

public class GetAllSaleRequest
{
  public int Page { get; set; } = 1;
  public int PageSize { get; set; } = 10;
  public string? SearchTerm { get; set; }
  public string? SortBy { get; set; }
  public bool SortDescending { get; set; }
  public bool? IsCancelled { get; set; }
}
