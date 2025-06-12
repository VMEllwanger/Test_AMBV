using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;

public class GetAllSaleCommand : IRequest<GetAllSaleResult>
{
  public int Page { get; set; } = 1;
  public int PageSize { get; set; } = 10;
  public string? SearchTerm { get; set; }
  public string? OrderBy { get; set; }
  public bool Ascending { get; set; } = true;
  public DateTime? StartDate { get; set; }
  public DateTime? EndDate { get; set; }
  public bool? IsCancelled { get; set; }
}
