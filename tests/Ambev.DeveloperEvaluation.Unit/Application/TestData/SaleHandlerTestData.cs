using Ambev.DeveloperEvaluation.Application.Sales.CancelItem;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateSaleCommand entities.
    /// The generated commands will have valid:
    /// - SaleNumber (sequential number)
    /// - Customer (name and document)
    /// - Branch (name and code)
    /// - Items (list of valid sale items)
    /// </summary>
    private static readonly Faker<CreateSaleCommand> createSaleCommandFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.SaleNumber, (f, s) => f.Random.Number(1, 1000).ToString())
        .RuleFor(s => s.Customer, f => f.Person.FullName)
        .RuleFor(s => s.Branch, f => f.Company.CompanyName())
        .RuleFor(s => s.Items, f => Enumerable.Range(1, f.Random.Number(1, 5))
            .Select(_ => new CreateSaleItemCommand
            {
                ProductId = f.Random.Guid().ToString(),
                ProductName = f.Commerce.ProductName(),
                Quantity = f.Random.Number(1, 10),
                UnitPrice = f.Random.Decimal(10, 1000),
            }).ToList());

    /// <summary>
    /// Configures the Faker to generate valid UpdateSaleCommand entities.
    /// The generated commands will have valid:
    /// - Id (GUID)
    /// - Customer (name and document)
    /// - Branch (name and code)
    /// - Items (list of valid sale items)
    /// </summary>
    private static readonly Faker<UpdateSaleCommand> updateSaleCommandFaker = new Faker<UpdateSaleCommand>()
        .RuleFor(s => s.Id, f => f.Random.Guid())
        .RuleFor(s => s.Customer, f => f.Person.FullName)
        .RuleFor(s => s.Branch, f => f.Company.CompanyName())
        .RuleFor(s => s.Items, f => Enumerable.Range(1, f.Random.Number(1, 5))
            .Select(_ => new UpdateSaleItemCommand
            {
                Id = f.Random.Guid(),
                ProductId = f.Random.Guid().ToString(),
                Quantity = f.Random.Number(1, 10),
                UnitPrice = f.Random.Decimal(10, 1000)
            }).ToList());

    /// <summary>
    /// Configures the Faker to generate valid GetSaleCommand entities.
    /// The generated commands will have valid:
    /// - Id (GUID)
    /// </summary>
    private static readonly Faker<GetSaleCommand> getSaleCommandFaker = new Faker<GetSaleCommand>()
        .RuleFor(s => s.Id, f => f.Random.Guid());

    /// <summary>
    /// Configures the Faker to generate valid GetAllSaleCommand entities.
    /// The generated commands will have valid:
    /// - Page (positive number)
    /// - PageSize (positive number between 1 and 100)
    /// </summary>
    private static readonly Faker<GetAllSaleCommand> getAllSaleCommandFaker = new Faker<GetAllSaleCommand>()
        .RuleFor(s => s.Page, f => f.Random.Number(1, 10))
        .RuleFor(s => s.PageSize, f => f.Random.Number(1, 100));

    /// <summary>
    /// Configures the Faker to generate valid CancelSaleCommand entities.
    /// The generated commands will have valid:
    /// - Id (GUID)
    /// - Reason (non-empty string)
    /// </summary>
    private static readonly Faker<CancelSaleCommand> cancelSaleCommandFaker = new Faker<CancelSaleCommand>()
        .RuleFor(s => s.Id, f => f.Random.Guid())
        .RuleFor(s => s.CancellationReason, f => f.Lorem.Sentence());

    /// <summary>
    /// Configures the Faker to generate valid CancelItemCommand entities.
    /// The generated commands will have valid:
    /// - SaleId (GUID)
    /// - ItemId (GUID)
    /// - Reason (non-empty string)
    /// </summary>
    private static readonly Faker<CancelItemCommand> cancelItemCommandFaker = new Faker<CancelItemCommand>()
        .RuleFor(s => s.SaleId, f => f.Random.Guid())
        .RuleFor(s => s.ItemId, f => f.Random.Guid());

    /// <summary>
    /// Generates a valid CreateSaleCommand with randomized data.
    /// </summary>
    public static CreateSaleCommand GenerateValidCreateSaleCommand()
    {
        return createSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid UpdateSaleCommand with randomized data.
    /// </summary>
    public static UpdateSaleCommand GenerateValidUpdateSaleCommand()
    {
        return updateSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid GetSaleCommand with randomized data.
    /// </summary>
    public static GetSaleCommand GenerateValidGetSaleCommand()
    {
        return getSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid GetAllSaleCommand with randomized data.
    /// </summary>
    public static GetAllSaleCommand GenerateValidGetAllSaleCommand()
    {
        return getAllSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid CancelSaleCommand with randomized data.
    /// </summary>
    public static CancelSaleCommand GenerateValidCancelSaleCommand()
    {
        return cancelSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid CancelItemCommand with randomized data.
    /// </summary>
    public static CancelItemCommand GenerateValidCancelItemCommand()
    {
        return cancelItemCommandFaker.Generate();
    }
}
