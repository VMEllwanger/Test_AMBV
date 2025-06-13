using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult>
{
  private readonly ISaleRepository _saleRepository;
  private readonly IMapper _mapper;
  private readonly IValidator<GetSaleCommand> _validator;
  private readonly ILogger<GetSaleHandler> _logger;

  public GetSaleHandler(
      ISaleRepository saleRepository,
      IMapper mapper,
      IValidator<GetSaleCommand> validator,
      ILogger<GetSaleHandler> logger)
  {
    _saleRepository = saleRepository;
    _mapper = mapper;
    _validator = validator;
    _logger = logger;
  }

  public async Task<GetSaleResult> Handle(GetSaleCommand request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Starting to retrieve sale with ID: {SaleId}", request.Id);

    var validationResult = await _validator.ValidateAsync(request, cancellationToken);
    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Validation failed for sale retrieval: {Errors}", validationResult.Errors);
      throw new ValidationException(validationResult.Errors);
    }

    _logger.LogDebug("Fetching sale from repository");
    var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
    if (sale == null)
    {
      _logger.LogWarning("Sale not found with ID: {SaleId}", request.Id);
      throw new KeyNotFoundException($"Sale with ID {request.Id} not found.");
    }

    _logger.LogInformation("Sale retrieved successfully for ID: {SaleId}", sale.Id);
    return _mapper.Map<GetSaleResult>(sale);
  }
}
