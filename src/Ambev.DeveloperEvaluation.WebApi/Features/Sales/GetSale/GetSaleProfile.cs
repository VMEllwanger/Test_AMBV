using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public class GetSaleProfile : Profile
{
    public GetSaleProfile()
    {
        CreateMap<GetSaleRequest, GetSaleCommand>();
        CreateMap<Guid, GetSaleCommand>()
            .ConstructUsing(id => new GetSaleCommand() { Id = id });
        CreateMap<GetSaleResult, GetSaleResponse>();
        CreateMap<GetSaleItemResult, GetSaleItemResponse>();
    }
}
