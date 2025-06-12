using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
  private readonly ISaleRepository _saleRepository;
  private readonly IMapper _mapper;
  private readonly IValidator<UpdateSaleCommand> _validator;

  public UpdateSaleHandler(
      ISaleRepository saleRepository,
      IMapper mapper,
      IValidator<UpdateSaleCommand> validator)
  {
    _saleRepository = saleRepository;
    _mapper = mapper;
    _validator = validator;
  }

  public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
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

    if (sale.IsCancelled)
    {
      throw new InvalidOperationException("Não é possível atualizar uma venda cancelada.");
    }

    sale.Customer = request.Customer;
    sale.Branch = request.Branch;
    sale.Items = _mapper.Map<List<SaleItem>>(request.Items);

    await _saleRepository.UpdateAsync(sale, cancellationToken);

    return _mapper.Map<UpdateSaleResult>(sale);
  }
}
