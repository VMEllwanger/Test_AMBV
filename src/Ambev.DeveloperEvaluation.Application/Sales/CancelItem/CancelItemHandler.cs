using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

public class CancelItemHandler : IRequestHandler<CancelItemCommand, CancelItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CancelItemCommand> _validator;
    private readonly ILogger<CancelItemHandler> _logger;
    private readonly ISaleEventPublisher _eventPublisher;

    public CancelItemHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        IValidator<CancelItemCommand> validator,
        ILogger<CancelItemHandler> logger,
        ISaleEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
        _eventPublisher = eventPublisher;
    }

    public async Task<CancelItemResult> Handle(CancelItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting to cancel item {ItemId} from sale {SaleId}", request.ItemId, request.SaleId);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for item cancellation: {Errors}", validationResult.Errors);
            throw new ValidationException(validationResult.Errors);
        }

        _logger.LogDebug("Fetching sale from repository");
        var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);
        if (sale == null)
        {
            _logger.LogWarning("Sale not found with ID: {SaleId}", request.SaleId);
            return new CancelItemResult
            {
                Success = false,
                Message = "Sale not found"
            };
        }

        if (sale.IsCancelled)
        {
            _logger.LogWarning("Cannot cancel item from cancelled sale with ID: {SaleId}", request.SaleId);
            return new CancelItemResult
            {
                Success = false,
                Message = "Cannot cancel an item from a cancelled sale"
            };
        }

        var item = sale.Items.FirstOrDefault(x => x.Id == request.ItemId);
        if (item == null)
        {
            _logger.LogWarning("Item not found with ID: {ItemId} in sale {SaleId}", request.ItemId, request.SaleId);
            return new CancelItemResult
            {
                Success = false,
                Message = "Item not found in sale"
            };
        }

        if (item.IsCancelled)
        {
            _logger.LogWarning("Item already cancelled with ID: {ItemId} in sale {SaleId}", request.ItemId, request.SaleId);
            return new CancelItemResult
            {
                Success = false,
                Message = "Item is already cancelled"
            };
        }

        _logger.LogDebug("Cancelling item");
        item.IsCancelled = true;

        _logger.LogInformation("Saving updated sale to repository");
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        _logger.LogInformation("Publishing ItemCancelled event");
        await _eventPublisher.PublishItemCancelledAsync(new ItemCancelledEvent(
            sale.Id,
            sale.SaleNumber,
            item.Id,
            item.ProductId,
            item.Quantity,
            item.UnitPrice,
            DateTime.UtcNow));

        _logger.LogInformation("Item cancelled successfully with ID: {ItemId} from sale {SaleId}", item.Id, sale.Id);
        return new CancelItemResult
        {
            Success = true,
            Message = "Item cancelled successfully"
        };
    }
}
