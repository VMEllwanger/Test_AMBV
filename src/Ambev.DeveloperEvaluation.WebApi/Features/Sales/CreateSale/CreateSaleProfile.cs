using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSale;

/// <summary>
/// Profile for mapping between Application and API CreateSale responses
/// </summary>
public class CreateSaleProfile : Profile
{
  /// <summary>
  /// Initializes the mappings for CreateSale feature
  /// </summary>
  public CreateSaleProfile()
  {
    CreateMap<CreateSaleRequest, CreateSaleCommand>();
    CreateMap<CreateSaleItemRequest, CreateSaleItemCommand>();
    CreateMap<CreateSaleResult, CreateSaleResponse>();
    CreateMap<CreateSaleItemResult, CreateSaleItemResponse>();
  }
}
