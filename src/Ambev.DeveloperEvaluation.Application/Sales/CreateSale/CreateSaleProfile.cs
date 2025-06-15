using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

[ExcludeFromCodeCoverage]
public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleCommand, Sale>();
        CreateMap<CreateSaleItemCommand, SaleItem>();
        CreateMap<Sale, CreateSaleResult>();
        CreateMap<SaleItem, CreateSaleItemResult>();
    }
}
