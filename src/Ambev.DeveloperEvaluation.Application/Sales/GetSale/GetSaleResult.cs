using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

[ExcludeFromCodeCoverage]
public class GetSaleResult
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
    public List<GetSaleItemResult> Items { get; set; } = new();
}

[ExcludeFromCodeCoverage]
public class GetSaleItemResult
{
    public Guid Id { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
}
