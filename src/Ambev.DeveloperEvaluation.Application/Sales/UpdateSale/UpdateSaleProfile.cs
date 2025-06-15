using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

[ExcludeFromCodeCoverage]
public class UpdateSaleProfile : Profile
{
  public UpdateSaleProfile()
  {
    CreateMap<UpdateSaleCommand, Sale>();
    CreateMap<UpdateSaleItemCommand, SaleItem>();
    CreateMap<Sale, UpdateSaleResult>();
    CreateMap<SaleItem, UpdateSaleItemResult>();
  }
}
