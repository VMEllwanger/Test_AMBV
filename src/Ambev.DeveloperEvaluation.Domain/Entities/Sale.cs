
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sales transaction containing customer, branch, item list and total.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Unique number to identify the sale.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Date when the sale was made.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Foreign key to the customer.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Denormalized name of the customer.
    /// </summary>
    public string CustomerName { get; set; } = default!;


    /// <summary>
    /// Foreign key to the branch where the sale occurred.
    /// </summary>
    public int BranchId { get; set; }

    /// <summary>
    /// Denormalized name of the branch.
    /// </summary>
    public string BranchName { get; set; } = default!;

    /// <summary>
    /// Indicates whether the sale was cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// List of items in this sale.
    /// </summary>
    public List<SaleItem> Items { get; set; } = new();
}
