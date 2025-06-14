using Ambev.DeveloperEvaluation.Domain.Constants;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelItemSale;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Validation;

public class CancelItemSaleRequestValidatorTests
{
  private readonly CancelItemSaleRequestValidator _validator;

  public CancelItemSaleRequestValidatorTests()
  {
    _validator = new CancelItemSaleRequestValidator();
  }

  [Fact(DisplayName = "Given valid request When validating Then should be valid")]
  public void Validate_ValidRequest_ShouldBeValid()
  {
    // Given
    var request = new CancelItemSaleRequest
    {
      Id = Guid.NewGuid()
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
    var request = new CancelItemSaleRequest
    {
      Id = Guid.Empty
    };

    // When
    var result = _validator.Validate(request);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.ItemIdRequired);
  }
}
