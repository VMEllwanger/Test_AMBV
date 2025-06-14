using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class CancelSaleValidatorTests
{
  private readonly CancelSaleValidator _validator;

  public CancelSaleValidatorTests()
  {
    _validator = new CancelSaleValidator();
  }

  [Fact(DisplayName = "Given valid command When validating Then should be valid")]
  public void Validate_ValidCommand_ShouldBeValid()
  {
    // Given
    var command = new CancelSaleCommand
    {
      Id = Guid.NewGuid(),
      CancellationReason = "Motivo do cancelamento"
    };

    // When
    var result = _validator.Validate(command);

    // Then
    result.IsValid.Should().BeTrue();
  }

  [Fact(DisplayName = "Given empty id When validating Then should be invalid")]
  public void Validate_EmptyId_ShouldBeInvalid()
  {
    // Given
    var command = new CancelSaleCommand
    {
      Id = Guid.Empty,
      CancellationReason = "Motivo do cancelamento"
    };

    // When
    var result = _validator.Validate(command);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.SaleIdRequired);
  }

  [Fact(DisplayName = "Given empty cancellation reason When validating Then should be invalid")]
  public void Validate_EmptyCancellationReason_ShouldBeInvalid()
  {
    // Given
    var command = new CancelSaleCommand
    {
      Id = Guid.NewGuid(),
      CancellationReason = string.Empty
    };

    // When
    var result = _validator.Validate(command);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.CancellationReasonRequired);
  }

  [Fact(DisplayName = "Given cancellation reason exceeding max length When validating Then should be invalid")]
  public void Validate_CancellationReasonExceedingMaxLength_ShouldBeInvalid()
  {
    // Given
    var command = new CancelSaleCommand
    {
      Id = Guid.NewGuid(),
      CancellationReason = new string('a', 501)
    };

    // When
    var result = _validator.Validate(command);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.CancellationReasonMaxLength);
  }
}
