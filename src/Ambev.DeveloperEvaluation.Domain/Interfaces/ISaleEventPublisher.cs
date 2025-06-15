using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Interfaces;

public interface ISaleEventPublisher
{
  Task PublishSaleCreatedAsync(SaleCreatedEvent @event);
  Task PublishSaleModifiedAsync(SaleModifiedEvent @event);
  Task PublishSaleCancelledAsync(SaleCancelledEvent @event);
  Task PublishItemCancelledAsync(ItemCancelledEvent @event);
}
