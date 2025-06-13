using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSale;

public class CreateSaleProfile : Profile
{
  public CreateSaleProfile()
  {
    CreateMap<CreateSaleRequest, CreateSaleCommand>();
    CreateMap<CreateSaleResult, CreateSaleResponse>();
    CreateMap<CreateSaleItemResult, CreateSaleItemResponse>();
  }
}
