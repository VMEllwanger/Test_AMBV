using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class SaleTestData
{
    public static Sale GenerateValidSale()
    {
        return new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = "123",
            Date = DateTime.UtcNow,
            Customer = "John Doe",
            Branch = "Branch 1",
            Items = new List<SaleItem>
            {
                SaleItemTestData.GenerateValidSaleItem()
            }
        };
    }
}
