using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class SaleItemTestData
{
  public static SaleItem GenerateValidSaleItem()
  {
    return new SaleItem
    {
      Id = Guid.NewGuid(),
      ProductId = "1",
      ProductName = "Product 1",
      Quantity = 1,
      UnitPrice = 10.0m,
      Discount = 0.0m
    };
  }
}
