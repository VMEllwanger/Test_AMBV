using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSale;

public class GetAllSaleProfile : Profile
{
  public GetAllSaleProfile()
  {
    CreateMap<GetAllSaleRequest, GetAllSaleCommand>();
    CreateMap<GetAllSaleResult, GetAllSaleResponse>()
      .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Data))
      .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalItems))
      .ForMember(dest => dest.PageCount, opt => opt.MapFrom(src => src.TotalPages))
      .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
      .ForMember(dest => dest.PageSize, opt => opt.Ignore());
    CreateMap<GetAllSaleItemResult, GetAllSaleItemResponse>()
      .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Date));
  }
}
