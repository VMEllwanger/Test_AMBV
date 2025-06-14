using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

[ExcludeFromCodeCoverage]
public class DeleteSaleResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
