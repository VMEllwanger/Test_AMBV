using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class CreateSaleValidatorTests
{
    private readonly CreateSaleValidator _validator;

    public CreateSaleValidatorTests()
    {
        _validator = new CreateSaleValidator();
    }

    [Fact(DisplayName = "Given valid command When validating Then should be valid")]
    public void Validate_ValidCommand_ShouldBeValid()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "123",
            Date = DateTime.UtcNow,
            Customer = "John Doe",
            Branch = "Branch 1",
            Items = new List<CreateSaleItemCommand>
            {
                new()
                {
                    ProductId = "1",
                    ProductName = "Product 1",
                    Quantity = 1,
                    UnitPrice = 10.0m
                }
            }
        };

        // When
        var result = _validator.Validate(command);

        // Then
        result.IsValid.Should().BeTrue();
    }



    [Fact(DisplayName = "Given empty customer When validating Then should be invalid")]
    public void Validate_EmptyCustomer_ShouldBeInvalid()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "123",
            Date = DateTime.UtcNow,
            Customer = string.Empty,
            Branch = "Branch 1",
            Items = new List<CreateSaleItemCommand>
            {
                new()
                {
                    ProductId = "1",
                    ProductName = "Product 1",
                    Quantity = 1,
                    UnitPrice = 10.0m
                }
            }
        };

        // When
        var result = _validator.Validate(command);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.CustomerRequired);
    }

    [Fact(DisplayName = "Given empty branch When validating Then should be invalid")]
    public void Validate_EmptyBranch_ShouldBeInvalid()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "123",
            Date = DateTime.UtcNow,
            Customer = "John Doe",
            Branch = string.Empty,
            Items = new List<CreateSaleItemCommand>
            {
                new()
                {
                    ProductId = "1",
                    ProductName = "Product 1",
                    Quantity = 1,
                    UnitPrice = 10.0m
                }
            }
        };

        // When
        var result = _validator.Validate(command);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.BranchRequired);
    }

    [Fact(DisplayName = "Given empty items When validating Then should be invalid")]
    public void Validate_EmptyItems_ShouldBeInvalid()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "123",
            Date = DateTime.UtcNow,
            Customer = "John Doe",
            Branch = "Branch 1",
            Items = new List<CreateSaleItemCommand>()
        };

        // When
        var result = _validator.Validate(command);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.SaleItemsRequired);
    }

    [Fact(DisplayName = "Given invalid item When validating Then should be invalid")]
    public void Validate_InvalidItem_ShouldBeInvalid()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "123",
            Date = DateTime.UtcNow,
            Customer = "John Doe",
            Branch = "Branch 1",
            Items = new List<CreateSaleItemCommand>
            {
                new()
                {
                    ProductId = string.Empty,
                    ProductName = string.Empty,
                    Quantity = 0,
                    UnitPrice = 0
                }
            }
        };

        // When
        var result = _validator.Validate(command);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.ProductIdRequired);
        result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.QuantityGreaterThanZero);
        result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.UnitPriceGreaterThanZero);
    }
}
