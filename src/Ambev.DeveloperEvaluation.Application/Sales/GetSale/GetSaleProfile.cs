using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

[ExcludeFromCodeCoverage]
public class GetSaleProfile : Profile
{
  public GetSaleProfile()
  {
    CreateMap<Sale, GetSaleResult>();
    CreateMap<SaleItem, GetSaleItemResult>();
  }
}
