using Ambev.DeveloperEvaluation.Application.Sales.CancelItem;
using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class CancelItemValidatorTests
{
  private readonly CancelItemValidator _validator;

  public CancelItemValidatorTests()
  {
    _validator = new CancelItemValidator();
  }

  [Fact(DisplayName = "Given valid command When validating Then should be valid")]
  public void Validate_ValidCommand_ShouldBeValid()
  {
    // Given
    var command = new CancelItemCommand
    {
      SaleId = Guid.NewGuid(),
      ItemId = Guid.NewGuid()
    };

    // When
    var result = _validator.Validate(command);

    // Then
    result.IsValid.Should().BeTrue();
  }

  [Fact(DisplayName = "Given empty sale ID When validating Then should be invalid")]
  public void Validate_EmptySaleId_ShouldBeInvalid()
  {
    // Given
    var command = new CancelItemCommand
    {
      SaleId = Guid.Empty,
      ItemId = Guid.NewGuid()
    };

    // When
    var result = _validator.Validate(command);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.SaleIdRequired);
  }

  [Fact(DisplayName = "Given empty item ID When validating Then should be invalid")]
  public void Validate_EmptyItemId_ShouldBeInvalid()
  {
    // Given
    var command = new CancelItemCommand
    {
      SaleId = Guid.NewGuid(),
      ItemId = Guid.Empty
    };

    // When
    var result = _validator.Validate(command);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.ItemIdRequired);
  }
}
