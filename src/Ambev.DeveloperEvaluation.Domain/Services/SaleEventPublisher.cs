using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Infrastructure.Services;

public class SaleEventPublisher : ISaleEventPublisher
{
    private readonly ILogger<SaleEventPublisher> _logger;

    public SaleEventPublisher(ILogger<SaleEventPublisher> logger)
    {
        _logger = logger;
    }

    public Task PublishSaleCreatedAsync(SaleCreatedEvent @event)
    {
        _logger.LogInformation(
            "Event Published: SaleCreated | SaleId: {SaleId} | SaleNumber: {SaleNumber} | Customer: {Customer} | Branch: {Branch} | TotalAmount: {TotalAmount}",
            @event.SaleId,
            @event.SaleNumber,
            @event.Customer,
            @event.Branch,
            @event.TotalAmount);

        return Task.CompletedTask;
    }

    public Task PublishSaleModifiedAsync(SaleModifiedEvent @event)
    {
        _logger.LogInformation(
            "Event Published: SaleModified | SaleId: {SaleId} | SaleNumber: {SaleNumber} | Customer: {Customer} | Branch: {Branch} | TotalAmount: {TotalAmount}",
            @event.SaleId,
            @event.SaleNumber,
            @event.Customer,
            @event.Branch,
            @event.TotalAmount);

        return Task.CompletedTask;
    }

    public Task PublishSaleCancelledAsync(SaleCancelledEvent @event)
    {
        _logger.LogInformation(
            "Event Published: SaleCancelled | SaleId: {SaleId} | SaleNumber: {SaleNumber} | Customer: {Customer} | Branch: {Branch} | TotalAmount: {TotalAmount} | Reason: {Reason}",
            @event.SaleId,
            @event.SaleNumber,
            @event.Customer,
            @event.Branch,
            @event.TotalAmount,
            @event.CancellationReason);

        return Task.CompletedTask;
    }

    public Task PublishItemCancelledAsync(ItemCancelledEvent @event)
    {
        _logger.LogInformation(
            "Event Published: ItemCancelled | SaleId: {SaleId} | SaleNumber: {SaleNumber} | ItemId: {ItemId} | ProductId: {ProductId} | Quantity: {Quantity} | UnitPrice: {UnitPrice}",
            @event.SaleId,
            @event.SaleNumber,
            @event.ItemId,
            @event.ProductId,
            @event.Quantity,
            @event.UnitPrice);

        return Task.CompletedTask;
    }
}