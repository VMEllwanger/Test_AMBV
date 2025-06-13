namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCancelledEvent
{
  public Guid SaleId { get; }
  public string SaleNumber { get; }
  public string Customer { get; }
  public string Branch { get; }
  public decimal TotalAmount { get; }
  public DateTime CancelledAt { get; }
  public string CancellationReason { get; }

  public SaleCancelledEvent(
      Guid saleId,
      string saleNumber,
      string customer,
      string branch,
      decimal totalAmount,
      DateTime cancelledAt,
      string cancellationReason)
  {
    SaleId = saleId;
    SaleNumber = saleNumber;
    Customer = customer;
    Branch = branch;
    TotalAmount = totalAmount;
    CancelledAt = cancelledAt;
    CancellationReason = cancellationReason;
  }
}
