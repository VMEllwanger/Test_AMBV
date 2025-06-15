using Ambev.DeveloperEvaluation.Domain.Constants;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Validation;

public class CancelSaleRequestValidatorTests
{
    private readonly CancelSaleRequestValidator _validator;

    public CancelSaleRequestValidatorTests()
    {
        _validator = new CancelSaleRequestValidator();
    }

    [Fact(DisplayName = "Given valid request When validating Then should be valid")]
    public void Validate_ValidRequest_ShouldBeValid()
    {
        // Given
        var request = new CancelSaleRequest
        {
            Id = Guid.NewGuid(),
            CancellationReason = "Customer requested cancellation"
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
        var request = new CancelSaleRequest
        {
            Id = Guid.Empty,
            CancellationReason = "Customer requested cancellation"
        };

        // When
        var result = _validator.Validate(request);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.SaleIdRequired);
    }

    [Fact(DisplayName = "Given empty CancellationReason When validating Then should be invalid")]
    public void Validate_EmptyCancellationReason_ShouldBeInvalid()
    {
        // Given
        var request = new CancelSaleRequest
        {
            Id = Guid.NewGuid(),
        };

        // When
        var result = _validator.Validate(request);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.CancellationReasonRequired);
    }

    [Fact(DisplayName = "Given CancellationReason exceeding maximum length When validating Then should be invalid")]
    public void Validate_CancellationReasonExceedingMaxLength_ShouldBeInvalid()
    {
        // Given
        var request = new CancelSaleRequest
        {
            Id = Guid.NewGuid(),
            CancellationReason = new string('a', 501)
        };

        // When
        var result = _validator.Validate(request);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.CancellationReasonMaxLength);
    }
}
