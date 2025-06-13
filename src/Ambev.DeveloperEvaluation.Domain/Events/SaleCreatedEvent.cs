namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCreatedEvent
{
  public Guid SaleId { get; }
  public string SaleNumber { get; }
  public string Customer { get; }
  public string Branch { get; }
  public decimal TotalAmount { get; }
  public DateTime CreatedAt { get; }

  public SaleCreatedEvent(
      Guid saleId,
      string saleNumber,
      string customer,
      string branch,
      decimal totalAmount,
      DateTime createdAt)
  {
    SaleId = saleId;
    SaleNumber = saleNumber;
    Customer = customer;
    Branch = branch;
    TotalAmount = totalAmount;
    CreatedAt = createdAt;
  }
}
