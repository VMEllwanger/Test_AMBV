using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class GetSaleValidatorTests
{
  private readonly GetSaleValidator _validator;

  public GetSaleValidatorTests()
  {
    _validator = new GetSaleValidator();
  }

  [Fact(DisplayName = "Given valid command When validating Then should be valid")]
  public void Validate_ValidCommand_ShouldBeValid()
  {
    // Given
    var command = new GetSaleCommand
    {
      Id = Guid.NewGuid()
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
    var command = new GetSaleCommand
    {
      Id = Guid.Empty
    };

    // When
    var result = _validator.Validate(command);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.SaleIdRequired);
  }
}
