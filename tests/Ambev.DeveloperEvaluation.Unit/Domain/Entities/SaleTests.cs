using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// Tests cover validation scenarios and business rules.
/// </summary>
public class SaleTests
{
    private readonly SaleValidator _validator;

    public SaleTests()
    {
        _validator = new SaleValidator();
    }

    /// <summary>
    /// Tests that when a sale is cancelled, its status changes to cancelled.
    /// </summary>
    [Fact(DisplayName = "Sale status should change to cancelled when cancelled")]
    public void Given_ActiveSale_When_Cancelled_Then_StatusShouldBeCancelled()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.IsCancelled = false;

        // Act
        sale.Cancel();

        // Assert
        Assert.True(sale.IsCancelled);
        Assert.NotNull(sale.UpdatedAt);
    }

    /// <summary>
    /// Tests that total amount is calculated correctly.
    /// </summary>
    [Fact(DisplayName = "Total amount should be calculated correctly")]
    public void Given_ValidSale_When_CalculatingTotal_Then_ShouldReturnCorrectTotal()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Items = new List<SaleItem>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ProductId = "1",
                ProductName = "Product 1",
                Quantity = 2,
                UnitPrice = 10.0m,
                Discount = 0.1m,
                TotalAmount = 18.0m
            },
            new()
            {
                Id = Guid.NewGuid(),
                ProductId = "2",
                ProductName = "Product 2",
                Quantity = 1,
                UnitPrice = 20.0m,
                Discount = 0.2m,
                TotalAmount = 19.8m
            }
        };

        // Act
        sale.CalculateTotalAmount();

        // Assert
        Assert.Equal(37.8m, sale.TotalAmount);
        Assert.NotNull(sale.UpdatedAt);
    }

    /// <summary>
    /// Tests that discounts are applied correctly based on quantity rules.
    /// </summary>
    [Fact(DisplayName = "Discounts should be applied correctly based on quantity rules")]
    public void Given_ValidSale_When_ApplyingDiscounts_Then_ShouldApplyCorrectDiscounts()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Items = new List<SaleItem>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ProductId = "1",
                ProductName = "Product 1",
                Quantity = 15,
                UnitPrice = 10.0m,
                Discount = 0m
            },
            new()
            {
                Id = Guid.NewGuid(),
                ProductId = "2",
                ProductName = "Product 2",
                Quantity = 5,
                UnitPrice = 20.0m,
                Discount = 0m
            },
            new()
            {
                Id = Guid.NewGuid(),
                ProductId = "3",
                ProductName = "Product 3",
                Quantity = 2,
                UnitPrice = 30.0m,
                Discount = 0m
            }
        };

        // Act
        sale.ApplyDiscounts();

        // Assert
        Assert.Equal(0.20m, sale.Items[0].Discount);
        Assert.Equal(0.10m, sale.Items[1].Discount);
        Assert.Equal(0.00m, sale.Items[2].Discount);
        Assert.NotNull(sale.UpdatedAt);
    }

    /// <summary>
    /// Tests that validation passes when all sale properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid sale data")]
    public void Given_ValidSaleData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when sale properties are invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid sale data")]
    public void Given_InvalidSaleData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = "",
            Date = DateTime.MinValue,
            Customer = "",
            Branch = "",
            Items = new List<SaleItem>(),
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
}
