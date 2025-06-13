using Ambev.DeveloperEvaluation.Application.Sales.CancelItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelItemSale;

public class CancelItemSaleProfile : Profile
{
  public CancelItemSaleProfile()
  {
    CreateMap<CancelItemSaleRequest, CancelItemCommand>();
    CreateMap<CancelItemResult, CancelItemSaleResponse>();
  }
}
