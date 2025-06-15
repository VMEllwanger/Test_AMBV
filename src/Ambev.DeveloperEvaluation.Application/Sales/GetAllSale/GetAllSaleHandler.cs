using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;

public class GetAllSaleHandler : IRequestHandler<GetAllSaleCommand, GetAllSaleResult>
{
  private readonly ISaleRepository _saleRepository;
  private readonly IMapper _mapper;
  private readonly IValidator<GetAllSaleCommand> _validator;
  private readonly ILogger<GetAllSaleHandler> _logger;

  public GetAllSaleHandler(
      ISaleRepository saleRepository,
      IMapper mapper,
      IValidator<GetAllSaleCommand> validator,
      ILogger<GetAllSaleHandler> logger)
  {
    _saleRepository = saleRepository;
    _mapper = mapper;
    _validator = validator;
    _logger = logger;
  }

  public async Task<GetAllSaleResult> Handle(GetAllSaleCommand request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Starting to retrieve all sales with filters: Page={Page}, PageSize={PageSize}, SearchTerm={SearchTerm}, OrderBy={OrderBy}, Ascending={Ascending}",
        request.Page, request.PageSize, request.SearchTerm, request.OrderBy, request.Ascending);

    var validationResult = await _validator.ValidateAsync(request, cancellationToken);
    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Validation failed for sales retrieval: {Errors}", validationResult.Errors);
      throw new ValidationException(validationResult.Errors);
    }

    _logger.LogDebug("Fetching sales from repository");
    var sales = await _saleRepository.GetAllAsync(
        request.Page,
        request.PageSize,
        request.SearchTerm,
        request.OrderBy,
        request.Ascending,
        cancellationToken);

    _logger.LogInformation("Successfully retrieved {Count} sales", sales.TotalCount);
    return _mapper.Map<GetAllSaleResult>(sales);
  }
}
