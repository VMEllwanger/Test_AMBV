using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
  private readonly ISaleRepository _saleRepository;
  private readonly IMapper _mapper;
  private readonly IValidator<UpdateSaleCommand> _validator;
  private readonly ILogger<UpdateSaleHandler> _logger;

  public UpdateSaleHandler(
      ISaleRepository saleRepository,
      IMapper mapper,
      IValidator<UpdateSaleCommand> validator,
      ILogger<UpdateSaleHandler> logger)
  {
    _saleRepository = saleRepository;
    _mapper = mapper;
    _validator = validator;
    _logger = logger;
  }

  public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Starting to update sale with ID: {SaleId}", request.Id);

    var validationResult = await _validator.ValidateAsync(request, cancellationToken);
    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Validation failed for sale update: {Errors}", validationResult.Errors);
      throw new ValidationException(validationResult.Errors);
    }

    _logger.LogDebug("Fetching sale from repository");
    var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
    if (sale == null)
    {
      _logger.LogWarning("Sale not found with ID: {SaleId}", request.Id);
      throw new KeyNotFoundException($"Venda com ID {request.Id} não encontrada.");
    }

    if (sale.IsCancelled)
    {
      _logger.LogWarning("Attempted to update cancelled sale with ID: {SaleId}", request.Id);
      throw new InvalidOperationException("Não é possível atualizar uma venda cancelada.");
    }

    _logger.LogDebug("Updating sale properties");
    sale.Customer = request.Customer;
    sale.Branch = request.Branch;
    sale.Items = _mapper.Map<List<SaleItem>>(request.Items);

    _logger.LogInformation("Saving updated sale to repository");
    await _saleRepository.UpdateAsync(sale, cancellationToken);

    _logger.LogInformation("Sale updated successfully with ID: {SaleId}", sale.Id);
    return _mapper.Map<UpdateSaleResult>(sale);
  }
}
