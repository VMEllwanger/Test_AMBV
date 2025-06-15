using Ambev.DeveloperEvaluation.Domain.Constants;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Validation;

public class UpdateSaleRequestValidatorTests
{
  private readonly UpdateSaleRequestValidator _validator;

  public UpdateSaleRequestValidatorTests()
  {
    _validator = new UpdateSaleRequestValidator();
  }

  [Fact(DisplayName = "Given valid request When validating Then should be valid")]
  public void Validate_ValidRequest_ShouldBeValid()
  {
    // Given
    var request = new UpdateSaleRequest
    {
      Id = Guid.NewGuid(),
      Branch = "Branch 1",
      Customer = "John Doe",
      Items = new List<UpdateSaleItemRequest>
      {
        new()
        {
          Id = Guid.NewGuid(),
          ProductId = Guid.NewGuid(),
          Quantity = 1,
          UnitPrice = 10.0m
        }
      }
    };

    // When
    var result = _validator.Validate(request);

    // Then
    result.IsValid.Should().BeTrue();
  }

  [Fact(DisplayName = "Given empty id When validating Then should be invalid")]
  public void Validate_EmptyId_ShouldBeInvalid()
  {
    // Given
    var request = new UpdateSaleRequest
    {
      Id = Guid.Empty,
      Branch = "Branch 1",
      Customer = "John Doe",
      Items = new List<UpdateSaleItemRequest>()
    };

    // When
    var result = _validator.Validate(request);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.SaleIdRequired);
  }

  [Fact(DisplayName = "Given empty branch When validating Then should be invalid")]
  public void Validate_EmptyBranch_ShouldBeInvalid()
  {
    // Given
    var request = new UpdateSaleRequest
    {
      Id = Guid.NewGuid(),
      Branch = string.Empty,
      Customer = "John Doe",
      Items = new List<UpdateSaleItemRequest>()
    };

    // When
    var result = _validator.Validate(request);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.BranchRequired);
  }

  [Fact(DisplayName = "Given empty customer When validating Then should be invalid")]
  public void Validate_EmptyCustomer_ShouldBeInvalid()
  {
    // Given
    var request = new UpdateSaleRequest
    {
      Id = Guid.NewGuid(),
      Branch = "Branch 1",
      Customer = string.Empty,
      Items = new List<UpdateSaleItemRequest>()
    };

    // When
    var result = _validator.Validate(request);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.CustomerRequired);
  }

  [Fact(DisplayName = "Given empty items When validating Then should be invalid")]
  public void Validate_EmptyItems_ShouldBeInvalid()
  {
    // Given
    var request = new UpdateSaleRequest
    {
      Id = Guid.NewGuid(),
      Branch = "Branch 1",
      Customer = "John Doe",
      Items = new List<UpdateSaleItemRequest>()
    };

    // When
    var result = _validator.Validate(request);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.SaleItemsRequired);
  }
}

public class UpdateSaleItemRequestValidatorTests
{
  private readonly UpdateSaleItemRequestValidator _validator;

  public UpdateSaleItemRequestValidatorTests()
  {
    _validator = new UpdateSaleItemRequestValidator();
  }

  [Fact(DisplayName = "Given valid item When validating Then should be valid")]
  public void Validate_ValidItem_ShouldBeValid()
  {
    // Given
    var item = new UpdateSaleItemRequest
    {
      Id = Guid.NewGuid(),
      ProductId = Guid.NewGuid(),
      Quantity = 1,
      UnitPrice = 10.0m
    };

    // When
    var result = _validator.Validate(item);

    // Then
    result.IsValid.Should().BeTrue();
  }

  [Fact(DisplayName = "Given empty id When validating Then should be invalid")]
  public void Validate_EmptyId_ShouldBeInvalid()
  {
    // Given
    var item = new UpdateSaleItemRequest
    {
      Id = Guid.Empty,
      ProductId = Guid.NewGuid(),
      Quantity = 1,
      UnitPrice = 10.0m
    };

    // When
    var result = _validator.Validate(item);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.ItemIdRequired);
  }

  [Fact(DisplayName = "Given empty product id When validating Then should be invalid")]
  public void Validate_EmptyProductId_ShouldBeInvalid()
  {
    // Given
    var item = new UpdateSaleItemRequest
    {
      Id = Guid.NewGuid(),
      ProductId = Guid.Empty,
      Quantity = 1,
      UnitPrice = 10.0m
    };

    // When
    var result = _validator.Validate(item);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.ProductIdRequired);
  }

  [Fact(DisplayName = "Given zero quantity When validating Then should be invalid")]
  public void Validate_ZeroQuantity_ShouldBeInvalid()
  {
    // Given
    var item = new UpdateSaleItemRequest
    {
      Id = Guid.NewGuid(),
      ProductId = Guid.NewGuid(),
      Quantity = 0,
      UnitPrice = 10.0m
    };

    // When
    var result = _validator.Validate(item);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.QuantityGreaterThanZero);
  }

  [Fact(DisplayName = "Given zero unit price When validating Then should be invalid")]
  public void Validate_ZeroUnitPrice_ShouldBeInvalid()
  {
    // Given
    var item = new UpdateSaleItemRequest
    {
      Id = Guid.NewGuid(),
      ProductId = Guid.NewGuid(),
      Quantity = 1,
      UnitPrice = 0.0m
    };

    // When
    var result = _validator.Validate(item);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.UnitPriceGreaterThanZero);
  }
}
