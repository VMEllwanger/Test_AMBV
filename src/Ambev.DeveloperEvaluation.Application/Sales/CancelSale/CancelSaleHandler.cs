using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Constants;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
  private readonly ISaleRepository _saleRepository;
  private readonly IMapper _mapper;
  private readonly IValidator<CancelSaleCommand> _validator;
  private readonly ILogger<CancelSaleHandler> _logger;
  private readonly ISaleEventPublisher _eventPublisher;

  public CancelSaleHandler(
      ISaleRepository saleRepository,
      IMapper mapper,
      IValidator<CancelSaleCommand> validator,
      ILogger<CancelSaleHandler> logger,
      ISaleEventPublisher eventPublisher)
  {
    _saleRepository = saleRepository;
    _mapper = mapper;
    _validator = validator;
    _logger = logger;
    _eventPublisher = eventPublisher;
  }

  public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Starting to cancel sale with ID: {SaleId}", request.Id);

    var validationResult = await _validator.ValidateAsync(request, cancellationToken);
    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Validation failed for sale cancellation: {Errors}", validationResult.Errors);
      throw new ValidationException(validationResult.Errors);
    }

    _logger.LogDebug("Fetching sale from repository");
    var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
    if (sale == null)
    {
      _logger.LogWarning("Sale not found with ID: {SaleId}", request.Id);
      return new CancelSaleResult
      {
        Success = false,
        Message = string.Format(ApiMessages.SaleNotFound, request.Id)
      };
    }

    if (sale.IsCancelled)
    {
      _logger.LogWarning("Sale already cancelled with ID: {SaleId}", request.Id);
      return new CancelSaleResult
      {
        Success = false,
        Message = ApiMessages.SaleAlreadyCancelled
      };
    }

    _logger.LogDebug("Cancelling sale");
    sale.Cancel();

    _logger.LogInformation("Saving cancelled sale to repository");
    await _saleRepository.UpdateAsync(sale, cancellationToken);

    _logger.LogInformation("Publishing SaleCancelled event");
    await _eventPublisher.PublishSaleCancelledAsync(new SaleCancelledEvent(
        sale.Id,
        sale.SaleNumber,
        sale.Customer,
        sale.Branch,
        sale.TotalAmount,
        DateTime.UtcNow,
        request.CancellationReason));

    _logger.LogInformation("Sale cancelled successfully with ID: {SaleId}", sale.Id);
    return new CancelSaleResult
    {
      Success = true,
      Message = ApiMessages.SaleCancelled
    };
  }
}
