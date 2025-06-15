using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleProfile : Profile
{
  public UpdateSaleProfile()
  {
    CreateMap<UpdateSaleRequest, UpdateSaleCommand>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
      .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
      .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
      .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

    CreateMap<UpdateSaleItemRequest, UpdateSaleItemCommand>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
      .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId.ToString()))
      .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
      .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice));

    CreateMap<UpdateSaleResult, UpdateSaleResponse>();
    CreateMap<UpdateSaleItemResult, UpdateSaleItemResponse>();
  }
}
