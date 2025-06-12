using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
  private readonly ISaleRepository _saleRepository;
  private readonly IMapper _mapper;
  private readonly IValidator<CreateSaleCommand> _validator;

  public CreateSaleHandler(
      ISaleRepository saleRepository,
      IMapper mapper,
      IValidator<CreateSaleCommand> validator)
  {
    _saleRepository = saleRepository;
    _mapper = mapper;
    _validator = validator;
  }

  public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
  {
    var validationResult = await _validator.ValidateAsync(request, cancellationToken);
    if (!validationResult.IsValid)
      throw new ValidationException(validationResult.Errors);

    var sale = _mapper.Map<Sale>(request);
    sale.ApplyDiscounts();

    var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
    return _mapper.Map<CreateSaleResult>(createdSale);
  }
}
