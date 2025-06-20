using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;
[ExcludeFromCodeCoverage]
public class CancelItemResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
