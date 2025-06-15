using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in a sale with its details and calculations.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets the product identifier.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// Gets the product name.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets the quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets the discount percentage applied to this item.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets the total amount for this item (including discount).
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets the foreign key to the sale.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets the navigation property to the sale.
    /// </summary>
    public Sale Sale { get; set; } = null!;

    /// <summary>
    /// Gets whether the item is cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Initializes a new instance of the SaleItem class.
    /// </summary>
    public SaleItem()
    {
    }

    /// <summary>
    /// Calculates the total amount for this item including discount.
    /// </summary>
    public void CalculateTotalAmount()
    {
        var subtotal = Quantity * UnitPrice;
        var discountAmount = subtotal * Discount;
        TotalAmount = subtotal - discountAmount;
    }
}
