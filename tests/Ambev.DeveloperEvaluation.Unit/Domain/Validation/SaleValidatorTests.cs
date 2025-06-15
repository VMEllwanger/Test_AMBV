using Ambev.DeveloperEvaluation.Domain.Constants;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class SaleValidatorTests
{
  private readonly SaleValidator _validator;

  public SaleValidatorTests()
  {
    _validator = new SaleValidator();
  }

  [Fact(DisplayName = "Given valid sale When validating Then should be valid")]
  public void Validate_ValidSale_ShouldBeValid()
  {
    // Given
    var sale = new Sale
    {
      Id = Guid.NewGuid(),
      SaleNumber = "123",
      Date = DateTime.UtcNow,
      Customer = "John Doe",
      Branch = "Branch 1",
      Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = "1",
                    ProductName = "Product 1",
                    Quantity = 1,
                    UnitPrice = 10.0m,
                    Discount = 0.0m
                }
            }
    };

    // When
    var result = _validator.Validate(sale);

    // Then
    result.IsValid.Should().BeTrue();
  }

  [Fact(DisplayName = "Given empty sale number When validating Then should be invalid")]
  public void Validate_EmptySaleNumber_ShouldBeInvalid()
  {
    // Given
    var sale = new Sale
    {
      Id = Guid.NewGuid(),
      SaleNumber = string.Empty,
      Date = DateTime.UtcNow,
      Customer = "John Doe",
      Branch = "Branch 1",
      Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = "1",
                    ProductName = "Product 1",
                    Quantity = 1,
                    UnitPrice = 10.0m,
                    Discount = 0.0m
                }
            }
    };

    // When
    var result = _validator.Validate(sale);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.SaleNumberRequired);
  }

  [Fact(DisplayName = "Given empty date When validating Then should be invalid")]
  public void Validate_EmptyDate_ShouldBeInvalid()
  {
    // Given
    var sale = new Sale
    {
      Id = Guid.NewGuid(),
      SaleNumber = "123",
      Date = DateTime.MinValue,
      Customer = "John Doe",
      Branch = "Branch 1",
      Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = "1",
                    ProductName = "Product 1",
                    Quantity = 1,
                    UnitPrice = 10.0m,
                    Discount = 0.0m
                }
            }
    };

    // When
    var result = _validator.Validate(sale);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.SaleDateRequired);
  }

  [Fact(DisplayName = "Given empty customer When validating Then should be invalid")]
  public void Validate_EmptyCustomer_ShouldBeInvalid()
  {
    // Given
    var sale = new Sale
    {
      Id = Guid.NewGuid(),
      SaleNumber = "123",
      Date = DateTime.UtcNow,
      Customer = string.Empty,
      Branch = "Branch 1",
      Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = "1",
                    ProductName = "Product 1",
                    Quantity = 1,
                    UnitPrice = 10.0m,
                    Discount = 0.0m
                }
            }
    };

    // When
    var result = _validator.Validate(sale);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.CustomerRequired);
  }

  [Fact(DisplayName = "Given empty branch When validating Then should be invalid")]
  public void Validate_EmptyBranch_ShouldBeInvalid()
  {
    // Given
    var sale = new Sale
    {
      Id = Guid.NewGuid(),
      SaleNumber = "123",
      Date = DateTime.UtcNow,
      Customer = "John Doe",
      Branch = string.Empty,
      Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = "1",
                    ProductName = "Product 1",
                    Quantity = 1,
                    UnitPrice = 10.0m,
                    Discount = 0.0m
                }
            }
    };

    // When
    var result = _validator.Validate(sale);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.BranchRequired);
  }

  [Fact(DisplayName = "Given empty items When validating Then should be invalid")]
  public void Validate_EmptyItems_ShouldBeInvalid()
  {
    // Given
    var sale = new Sale
    {
      Id = Guid.NewGuid(),
      SaleNumber = "123",
      Date = DateTime.UtcNow,
      Customer = "John Doe",
      Branch = "Branch 1",
      Items = new List<SaleItem>()
    };

    // When
    var result = _validator.Validate(sale);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.SaleItemsRequired);
  }

  [Fact(DisplayName = "Given invalid item When validating Then should be invalid")]
  public void Validate_InvalidItem_ShouldBeInvalid()
  {
    // Given
    var sale = new Sale
    {
      Id = Guid.NewGuid(),
      SaleNumber = "123",
      Date = DateTime.UtcNow,
      Customer = "John Doe",
      Branch = "Branch 1",
      Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = string.Empty,
                    ProductName = string.Empty,
                    Quantity = 0,
                    UnitPrice = 0,
                    Discount = -0.1m
                }
            }
    };

    // When
    var result = _validator.Validate(sale);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.ProductIdRequired);
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.ProductNameRequired);
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.QuantityGreaterThanZero);
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.UnitPriceGreaterThanZero);
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.DiscountCannotBeNegative);
  }
}
