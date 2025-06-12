using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResult>
{
  private readonly ISaleRepository _saleRepository;
  private readonly IMapper _mapper;
  private readonly IValidator<DeleteSaleCommand> _validator;

  public DeleteSaleHandler(
      ISaleRepository saleRepository,
      IMapper mapper,
      IValidator<DeleteSaleCommand> validator)
  {
    _saleRepository = saleRepository;
    _mapper = mapper;
    _validator = validator;
  }

  public async Task<DeleteSaleResult> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
  {
    var validationResult = await _validator.ValidateAsync(request, cancellationToken);
    if (!validationResult.IsValid)
      throw new ValidationException(validationResult.Errors);

    var success = await _saleRepository.DeleteAsync(request.Id, cancellationToken);

    return new DeleteSaleResult
    {
      Success = success,
      Message = success ? "Venda excluída com sucesso" : "Venda não encontrada"
    };
  }
}
