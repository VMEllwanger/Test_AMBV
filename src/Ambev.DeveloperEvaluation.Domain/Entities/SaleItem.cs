
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product sold in a sale transaction.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Foreign key to the parent sale.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Product Name.
    /// </summary>
    public string ProductName { get; set; } = default!;

    /// <summary>
    /// Quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Price per unit.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Discount applied to the item.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Total amount for this item (after discount).
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// ItemSaleCancelled flag.
    /// </summary>
    public bool IsCancelled { get; set; }

    public Sale Sale { get; set; } = default!;
}
