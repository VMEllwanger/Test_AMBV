using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;

[ExcludeFromCodeCoverage]
public class GetAllSaleProfile : Profile
{
  public GetAllSaleProfile()
  {
    CreateMap<Sale, GetAllSaleItemResult>();
    CreateMap<PaginatedResult<Sale>, GetAllSaleResult>()
        .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Items))
        .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalCount))
        .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.Page))
        .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages));
  }
}
