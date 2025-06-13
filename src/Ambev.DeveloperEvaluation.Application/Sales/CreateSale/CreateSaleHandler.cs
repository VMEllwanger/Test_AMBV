using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
  private readonly ISaleRepository _saleRepository;
  private readonly IMapper _mapper;
  private readonly IValidator<CreateSaleCommand> _validator;
  private readonly ILogger<CreateSaleHandler> _logger;
  private readonly ISaleEventPublisher _eventPublisher;

  public CreateSaleHandler(
      ISaleRepository saleRepository,
      IMapper mapper,
      IValidator<CreateSaleCommand> validator,
      ILogger<CreateSaleHandler> logger,
      ISaleEventPublisher eventPublisher)
  {
    _saleRepository = saleRepository;
    _mapper = mapper;
    _validator = validator;
    _logger = logger;
    _eventPublisher = eventPublisher;
  }

  public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Starting sale creation for customer: {Customer}", request.Customer);

    var validationResult = await _validator.ValidateAsync(request, cancellationToken);
    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Validation failed for sale creation: {Errors}", validationResult.Errors);
      throw new ValidationException(validationResult.Errors);
    }

    _logger.LogDebug("Mapping command to Sale entity");
    var sale = _mapper.Map<Sale>(request);

    _logger.LogDebug("Applying discounts to sale");
    sale.ApplyDiscounts();

    _logger.LogInformation("Creating sale in repository");
    var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

    _logger.LogInformation("Publishing SaleCreated event");
    await _eventPublisher.PublishSaleCreatedAsync(new SaleCreatedEvent(
        createdSale.Id,
        createdSale.SaleNumber,
        createdSale.Customer,
        createdSale.Branch,
        createdSale.TotalAmount,
        createdSale.CreatedAt));

    _logger.LogInformation("Sale created successfully with ID: {SaleId}", createdSale.Id);
    return _mapper.Map<CreateSaleResult>(createdSale);
  }
}
