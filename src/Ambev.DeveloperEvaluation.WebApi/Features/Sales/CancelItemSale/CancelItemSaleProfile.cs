using Ambev.DeveloperEvaluation.Application.Sales.CancelItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelItemSale;

public class CancelItemSaleProfile : Profile
{
  public CancelItemSaleProfile()
  {
    CreateMap<CancelItemSaleRequest, CancelItemCommand>()
        .ForMember(dest => dest.SaleId, opt => opt.MapFrom(src => src.SaleId))
        .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Id));
    CreateMap<CancelItemResult, CancelItemSaleResponse>();
  }
}
