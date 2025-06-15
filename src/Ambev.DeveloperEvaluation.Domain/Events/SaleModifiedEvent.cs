namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleModifiedEvent
{
  public Guid SaleId { get; }
  public string SaleNumber { get; }
  public string Customer { get; }
  public string Branch { get; }
  public decimal TotalAmount { get; }
  public DateTime ModifiedAt { get; }

  public SaleModifiedEvent(
      Guid saleId,
      string saleNumber,
      string customer,
      string branch,
      decimal totalAmount,
      DateTime modifiedAt)
  {
    SaleId = saleId;
    SaleNumber = saleNumber;
    Customer = customer;
    Branch = branch;
    TotalAmount = totalAmount;
    ModifiedAt = modifiedAt;
  }
}
