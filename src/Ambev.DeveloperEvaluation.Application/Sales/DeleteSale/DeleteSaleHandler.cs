using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResult>
{
  private readonly ISaleRepository _saleRepository;
  private readonly IMapper _mapper;
  private readonly IValidator<DeleteSaleCommand> _validator;
  private readonly ILogger<DeleteSaleHandler> _logger;

  public DeleteSaleHandler(
      ISaleRepository saleRepository,
      IMapper mapper,
      IValidator<DeleteSaleCommand> validator,
      ILogger<DeleteSaleHandler> logger)
  {
    _saleRepository = saleRepository;
    _mapper = mapper;
    _validator = validator;
    _logger = logger;
  }

  public async Task<DeleteSaleResult> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Starting to delete sale with ID: {SaleId}", request.Id);

    var validationResult = await _validator.ValidateAsync(request, cancellationToken);
    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Validation failed for sale deletion: {Errors}", validationResult.Errors);
      throw new ValidationException(validationResult.Errors);
    }

    _logger.LogDebug("Attempting to delete sale from repository");
    var success = await _saleRepository.DeleteAsync(request.Id, cancellationToken);

    if (success)
    {
      _logger.LogInformation("Sale deleted successfully with ID: {SaleId}", request.Id);
    }
    else
    {
      _logger.LogWarning("Sale not found for deletion with ID: {SaleId}", request.Id);
    }

    return new DeleteSaleResult
    {
      Success = success,
      Message = success ? "Venda excluída com sucesso" : "Venda não encontrada"
    };
  }
}
