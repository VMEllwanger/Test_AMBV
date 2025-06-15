using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the SaleItem entity class.
/// Tests cover validation scenarios and business rules.
/// </summary>
public class SaleItemTests
{
  private readonly SaleItemValidator _validator;

  public SaleItemTests()
  {
    _validator = new SaleItemValidator();
  }

  /// <summary>
  /// Tests that total amount is calculated correctly.
  /// </summary>
  [Fact(DisplayName = "Total amount should be calculated correctly")]
  public void Given_ValidSaleItem_When_CalculatingTotal_Then_ShouldReturnCorrectTotal()
  {
    // Arrange
    var saleItem = SaleItemTestData.GenerateValidSaleItem();
    saleItem.Quantity = 2;
    saleItem.UnitPrice = 10.0m;
    saleItem.Discount = 0.1m;

    // Act
    saleItem.CalculateTotalAmount();

    // Assert
    Assert.Equal(18.0m, saleItem.TotalAmount);
  }

  /// <summary>
  /// Tests that total amount is calculated correctly with no discount.
  /// </summary>
  [Fact(DisplayName = "Total amount should be calculated correctly with no discount")]
  public void Given_ValidSaleItem_When_CalculatingTotalWithNoDiscount_Then_ShouldReturnCorrectTotal()
  {
    // Arrange
    var saleItem = SaleItemTestData.GenerateValidSaleItem();
    saleItem.Quantity = 2;
    saleItem.UnitPrice = 10.0m;
    saleItem.Discount = 0m;

    // Act
    saleItem.CalculateTotalAmount();

    // Assert
    Assert.Equal(20.0m, saleItem.TotalAmount);
  }

  /// <summary>
  /// Tests that total amount is calculated correctly with maximum discount.
  /// </summary>
  [Fact(DisplayName = "Total amount should be calculated correctly with maximum discount")]
  public void Given_ValidSaleItem_When_CalculatingTotalWithMaximumDiscount_Then_ShouldReturnCorrectTotal()
  {
    // Arrange
    var saleItem = SaleItemTestData.GenerateValidSaleItem();
    saleItem.Quantity = 2;
    saleItem.UnitPrice = 10.0m;
    saleItem.Discount = 1.0m;

    // Act
    saleItem.CalculateTotalAmount();

    // Assert
    Assert.Equal(0.0m, saleItem.TotalAmount);
  }

  /// <summary>
  /// Tests that validation passes when all sale item properties are valid.
  /// </summary>
  [Fact(DisplayName = "Validation should pass for valid sale item data")]
  public void Given_ValidSaleItemData_When_Validated_Then_ShouldReturnValid()
  {
    // Arrange
    var saleItem = SaleItemTestData.GenerateValidSaleItem();

    // Act
    var result = _validator.Validate(saleItem);

    // Assert
    Assert.True(result.IsValid);
    Assert.Empty(result.Errors);
  }

  /// <summary>
  /// Tests that validation fails when sale item properties are invalid.
  /// </summary>
  [Fact(DisplayName = "Validation should fail for invalid sale item data")]
  public void Given_InvalidSaleItemData_When_Validated_Then_ShouldReturnInvalid()
  {
    // Arrange
    var saleItem = new SaleItem
    {
      Id = Guid.NewGuid(),
      ProductId = "",
      ProductName = "",
      Quantity = 0,
      UnitPrice = 0,
      Discount = -0.1m
    };

    // Act
    var result = _validator.Validate(saleItem);

    // Assert
    Assert.False(result.IsValid);
    Assert.NotEmpty(result.Errors);
  }

  /// <summary>
  /// Tests that validation fails when quantity exceeds maximum limit.
  /// </summary>
  [Fact(DisplayName = "Validation should fail when quantity exceeds maximum limit")]
  public void Given_QuantityExceedsMaximum_When_Validated_Then_ShouldReturnInvalid()
  {
    // Arrange
    var saleItem = SaleItemTestData.GenerateValidSaleItem();
    saleItem.Quantity = 21;

    // Act
    var result = _validator.Validate(saleItem);

    // Assert
    Assert.False(result.IsValid);
    Assert.NotEmpty(result.Errors);
  }

  /// <summary>
  /// Tests that validation fails when discount exceeds maximum limit.
  /// </summary>
  [Fact(DisplayName = "Validation should fail when discount exceeds maximum limit")]
  public void Given_DiscountExceedsMaximum_When_Validated_Then_ShouldReturnInvalid()
  {
    // Arrange
    var saleItem = SaleItemTestData.GenerateValidSaleItem();
    saleItem.Discount = 1.1m;

    // Act
    var result = _validator.Validate(saleItem);

    // Assert
    Assert.False(result.IsValid);
    Assert.NotEmpty(result.Errors);
  }
}
