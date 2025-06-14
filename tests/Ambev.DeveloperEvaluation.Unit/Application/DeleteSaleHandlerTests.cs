using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Constants;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="DeleteSaleHandler"/> class.
/// </summary>
public class DeleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly DeleteSaleHandler _handler;
    private readonly IValidator<DeleteSaleCommand> _validator;
    private readonly ILogger<DeleteSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _validator = Substitute.For<IValidator<DeleteSaleCommand>>();
        _logger = Substitute.For<ILogger<DeleteSaleHandler>>();
        _handler = new DeleteSaleHandler(_saleRepository, _mapper, _validator, _logger);
    }

    /// <summary>
    /// Tests that a valid sale deletion request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale id When deleting sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = new DeleteSaleCommand { Id = Guid.NewGuid() };
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());

        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Message.Should().Be(ApiMessages.SaleDeleted);
        await _saleRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());

    }

    /// <summary>
    /// Tests that an invalid sale deletion request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale id When deleting sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new DeleteSaleCommand();
        var validationErrors = new List<ValidationFailure>
        {
          new("Id", ValidationMessages.SaleIdRequired)
        };
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationErrors));

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        var exception = await act.Should()
            .ThrowAsync<ValidationException>();

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "Id" && e.ErrorMessage == ValidationMessages.SaleIdRequired);

    }

    /// <summary>
    /// Tests that deleting a non-existent sale throws a KeyNotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale id When deleting sale Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentSale_ThrowsKeyNotFoundException()
    {
        // Given
        var command = new DeleteSaleCommand { Id = Guid.NewGuid() };
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());

        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        var exception = await act.Should()
            .ThrowAsync<KeyNotFoundException>();

        exception.Which.Message.Should().Be(string.Format(ApiMessages.SaleNotFound, command.Id));

    }
}
