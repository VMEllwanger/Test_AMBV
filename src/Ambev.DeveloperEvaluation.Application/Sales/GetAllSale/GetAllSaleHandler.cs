using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;

public class GetAllSaleHandler : IRequestHandler<GetAllSaleCommand, GetAllSaleResult>
{
  private readonly ISaleRepository _saleRepository;
  private readonly IMapper _mapper;
  private readonly IValidator<GetAllSaleCommand> _validator;

  public GetAllSaleHandler(
      ISaleRepository saleRepository,
      IMapper mapper,
      IValidator<GetAllSaleCommand> validator)
  {
    _saleRepository = saleRepository;
    _mapper = mapper;
    _validator = validator;
  }

  public async Task<GetAllSaleResult> Handle(GetAllSaleCommand request, CancellationToken cancellationToken)
  {
    var validationResult = await _validator.ValidateAsync(request, cancellationToken);
    if (!validationResult.IsValid)
    {
      throw new ValidationException(validationResult.Errors);
    }

    var sales = await _saleRepository.GetAllAsync(
        request.Page,
        request.PageSize,
        request.SearchTerm,
        request.OrderBy,
        request.Ascending,
        cancellationToken);

    return _mapper.Map<GetAllSaleResult>(sales);
  }
}
