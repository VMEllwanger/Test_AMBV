using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult>
{
  private readonly ISaleRepository _saleRepository;
  private readonly IMapper _mapper;
  private readonly IValidator<GetSaleCommand> _validator;

  public GetSaleHandler(
      ISaleRepository saleRepository,
      IMapper mapper,
      IValidator<GetSaleCommand> validator)
  {
    _saleRepository = saleRepository;
    _mapper = mapper;
    _validator = validator;
  }

  public async Task<GetSaleResult> Handle(GetSaleCommand request, CancellationToken cancellationToken)
  {
    var validationResult = await _validator.ValidateAsync(request, cancellationToken);
    if (!validationResult.IsValid)
    {
      throw new ValidationException(validationResult.Errors);
    }

    var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
    if (sale == null)
    {
      throw new KeyNotFoundException($"Venda com ID {request.Id} não encontrada.");
    }

    return _mapper.Map<GetSaleResult>(sale);
  }
}
