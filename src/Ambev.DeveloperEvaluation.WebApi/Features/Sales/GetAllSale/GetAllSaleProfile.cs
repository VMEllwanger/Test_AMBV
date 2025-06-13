using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetAllSale;

public class GetAllSaleProfile : Profile
{
  public GetAllSaleProfile()
  {
    CreateMap<GetAllSaleRequest, GetAllSaleCommand>();
    CreateMap<GetAllSaleResult, GetAllSaleResponse>();
    CreateMap<GetAllSaleItemResult, GetAllSaleItemResponse>();
  }
}
