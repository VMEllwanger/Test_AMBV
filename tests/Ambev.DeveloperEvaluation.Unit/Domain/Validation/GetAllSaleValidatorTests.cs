using Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;
using Ambev.DeveloperEvaluation.Domain.Constants;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class GetAllSaleValidatorTests
{
    private readonly GetAllSaleValidator _validator;

    public GetAllSaleValidatorTests()
    {
        _validator = new GetAllSaleValidator();
    }

    [Fact(DisplayName = "Given valid command When validating Then should be valid")]
    public void Validate_ValidCommand_ShouldBeValid()
    {
        // Given
        var command = new GetAllSaleCommand
        {
            Page = 1,
            PageSize = 10
        };

        // When
        var result = _validator.Validate(command);

        // Then
        result.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Given page less than one When validating Then should be invalid")]
    public void Validate_PageLessThanOne_ShouldBeInvalid()
    {
        // Given
        var command = new GetAllSaleCommand
        {
            Page = 0,
            PageSize = 10
        };

        // When
        var result = _validator.Validate(command);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.PageGreaterThanZero);
    }

    [Fact(DisplayName = "Given page size less than one When validating Then should be invalid")]
    public void Validate_PageSizeLessThanOne_ShouldBeInvalid()
    {
        // Given
        var command = new GetAllSaleCommand
        {
            Page = 1,
            PageSize = 0
        };

        // When
        var result = _validator.Validate(command);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.PageSizeGreaterThanZero);
    }

    [Fact(DisplayName = "Given page size greater than hundred When validating Then should be invalid")]
    public void Validate_PageSizeGreaterThanHundred_ShouldBeInvalid()
    {
        // Given
        var command = new GetAllSaleCommand
        {
            Page = 1,
            PageSize = 101
        };

        // When
        var result = _validator.Validate(command);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.PageSizeLessThanOrEqualHundred);
    }

    [Fact(DisplayName = "Given start date greater than end date When validating Then should be invalid")]
    public void Validate_StartDateGreaterThanEndDate_ShouldBeInvalid()
    {
        // Given
        var command = new GetAllSaleCommand
        {
            Page = 1,
            PageSize = 10,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow
        };

        // When
        var result = _validator.Validate(command);

        // Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ValidationMessages.StartDateLessThanEndDate);
    }
}

