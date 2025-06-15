namespace Ambev.DeveloperEvaluation.Domain.Events;

public class ItemCancelledEvent
{
  public Guid SaleId { get; }
  public string SaleNumber { get; }
  public Guid ItemId { get; }
  public string ProductId { get; }
  public int Quantity { get; }
  public decimal UnitPrice { get; }
  public DateTime CancelledAt { get; }

  public ItemCancelledEvent(
      Guid saleId,
      string saleNumber,
      Guid itemId,
      string productId,
      int quantity,
      decimal unitPrice,
      DateTime cancelledAt)
  {
    SaleId = saleId;
    SaleNumber = saleNumber;
    ItemId = itemId;
    ProductId = productId;
    Quantity = quantity;
    UnitPrice = unitPrice;
    CancelledAt = cancelledAt;
  }
}
