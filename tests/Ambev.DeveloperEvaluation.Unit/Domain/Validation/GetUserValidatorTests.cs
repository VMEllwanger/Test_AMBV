using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class GetUserValidatorTests
{
  private readonly GetUserValidator _validator;

  public GetUserValidatorTests()
  {
    _validator = new GetUserValidator();
  }

  [Fact(DisplayName = "Given valid command When validating Then should be valid")]
  public void Validate_ValidCommand_ShouldBeValid()
  {
    // Given
    var command = new GetUserCommand(Guid.NewGuid());

    // When
    var result = _validator.Validate(command);

    // Then
    result.IsValid.Should().BeTrue();
  }

  [Fact(DisplayName = "Given empty id When validating Then should be invalid")]
  public void Validate_EmptyId_ShouldBeInvalid()
  {
    // Given
    var command = new GetUserCommand(Guid.Empty);

    // When
    var result = _validator.Validate(command);

    // Then
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.UserIdRequired);
  }
}
