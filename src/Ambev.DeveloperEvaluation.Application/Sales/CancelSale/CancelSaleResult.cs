using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
[ExcludeFromCodeCoverage]
public class CancelSaleResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
