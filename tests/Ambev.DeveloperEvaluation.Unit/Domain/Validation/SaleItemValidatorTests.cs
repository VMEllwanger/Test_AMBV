using System.ComponentModel;
using Ambev.DeveloperEvaluation.Domain.Constants;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class SaleItemValidatorTests
{
  private readonly SaleItemValidator _validator;

  public SaleItemValidatorTests()
  {
    _validator = new SaleItemValidator();
  }

  [Fact(DisplayName = "Given valid sale item When validating Then should be valid")]
  public void Validate_ValidSaleItem_ShouldBeValid()
  {
    // Given
    var saleItem = new SaleItem
    {
      Id = Guid.NewGuid(),
      ProductId = "123",
      ProductName = "Test Product",
      Quantity = 1,
      UnitPrice = 10.0m,
      Discount = 0.0m
    };

    // When
    var result = _validator.Validate(saleItem);

    // Then
    result.IsValid.Should().BeTrue();
  }

  [Fact(DisplayName = "Given empty product id When validating Then should be invalid")]
  public void Validate_EmptyProductId_ShouldBeInvalid()
  {
    // Given
    var saleItem = new SaleItem
    {
      Id = Guid.NewGuid(),
      ProductId = string.Empty,
      ProductName = "Test Product",
      Quantity = 1,
      UnitPrice = 10.0m,
      Discount = 0.0m
    };

    // When
    var result = _validator.Validate(saleItem);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.ProductIdRequired);
  }

  [Fact(DisplayName = "Given empty product name When validating Then should be invalid")]
  public void Validate_EmptyProductName_ShouldBeInvalid()
  {
    // Given
    var saleItem = new SaleItem
    {
      Id = Guid.NewGuid(),
      ProductId = "123",
      ProductName = string.Empty,
      Quantity = 1,
      UnitPrice = 10.0m,
      Discount = 0.0m
    };

    // When
    var result = _validator.Validate(saleItem);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.ProductNameRequired);
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  [DisplayName("Given invalid quantity When validating Then should be invalid")]
  public void Validate_InvalidQuantity_ShouldBeInvalid(int quantity)
  {
    // Given
    var saleItem = new SaleItem
    {
      Id = Guid.NewGuid(),
      ProductId = "123",
      ProductName = "Test Product",
      Quantity = quantity,
      UnitPrice = 10.0m,
      Discount = 0.0m
    };

    // When
    var result = _validator.Validate(saleItem);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.QuantityGreaterThanZero);
  }

  [Fact(DisplayName = "Given quantity greater than 20 When validating Then should be invalid")]
  public void Validate_QuantityGreaterThanTwenty_ShouldBeInvalid()
  {
    // Given
    var saleItem = new SaleItem
    {
      Id = Guid.NewGuid(),
      ProductId = "123",
      ProductName = "Test Product",
      Quantity = 21,
      UnitPrice = 10.0m,
      Discount = 0.0m
    };

    // When
    var result = _validator.Validate(saleItem);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.QuantityMaxLimit);
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  [DisplayName("Given invalid unit price When validating Then should be invalid")]
  public void Validate_InvalidUnitPrice_ShouldBeInvalid(decimal unitPrice)
  {
    // Given
    var saleItem = new SaleItem
    {
      Id = Guid.NewGuid(),
      ProductId = "123",
      ProductName = "Test Product",
      Quantity = 1,
      UnitPrice = unitPrice,
      Discount = 0.0m
    };

    // When
    var result = _validator.Validate(saleItem);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.UnitPriceGreaterThanZero);
  }

  [Fact(DisplayName = "Given negative discount When validating Then should be invalid")]
  public void Validate_NegativeDiscount_ShouldBeInvalid()
  {
    // Given
    var saleItem = new SaleItem
    {
      Id = Guid.NewGuid(),
      ProductId = "123",
      ProductName = "Test Product",
      Quantity = 1,
      UnitPrice = 10.0m,
      Discount = -0.1m
    };

    // When
    var result = _validator.Validate(saleItem);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.DiscountCannotBeNegative);
  }

  [Fact(DisplayName = "Given discount greater than 100% When validating Then should be invalid")]
  public void Validate_DiscountGreaterThanHundred_ShouldBeInvalid()
  {
    // Given
    var saleItem = new SaleItem
    {
      Id = Guid.NewGuid(),
      ProductId = "123",
      ProductName = "Test Product",
      Quantity = 1,
      UnitPrice = 10.0m,
      Discount = 1.1m
    };

    // When
    var result = _validator.Validate(saleItem);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.DiscountMaxLimit);
  }
}
