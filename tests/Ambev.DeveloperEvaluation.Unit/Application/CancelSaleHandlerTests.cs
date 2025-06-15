using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Constants;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CancelSaleHandler"/> class.
/// </summary>
public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CancelSaleHandler _handler;
    private readonly ISaleEventPublisher _eventPublisher;
    private readonly IValidator<CancelSaleCommand> _validator;
    private readonly ILogger<CancelSaleHandler> _logger;


    /// <summary>
    /// Initializes a new instance of the <see cref="CancelSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventPublisher = Substitute.For<ISaleEventPublisher>();
        _validator = Substitute.For<IValidator<CancelSaleCommand>>();
        _logger = Substitute.For<ILogger<CancelSaleHandler>>();
        _handler = new CancelSaleHandler(_saleRepository, _mapper, _validator, _logger, _eventPublisher);
    }

    /// <summary>
    /// Tests that a valid sale cancellation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale id When cancelling sale Then returns success")]
    public async Task Handle_ValidRequest_ReturnsSuccess()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCancelSaleCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());
        var sale = new Sale
        {
            Id = command.Id,
            SaleNumber = "123",
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
                    UnitPrice = 100,
                    Discount = 0
                }
            }
        };

        var result = new CancelSaleResult
        {
            Success = true,
            Message = ApiMessages.SaleCancelled
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<CancelSaleResult>(sale).Returns(result);

        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        _eventPublisher.PublishSaleCancelledAsync(Arg.Any<SaleCancelledEvent>()).Returns(Task.CompletedTask);
        // When
        var cancelSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        cancelSaleResult.Should().NotBeNull();
        cancelSaleResult.Success.Should().BeTrue();
        cancelSaleResult.Message.Should().Be(ApiMessages.SaleCancelled);
        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<Sale>(s =>
                s.Id == sale.Id &&
                s.IsCancelled),
            Arg.Any<CancellationToken>());
        await _eventPublisher.Received(1).PublishSaleCancelledAsync(Arg.Any<SaleCancelledEvent>());
    }

    /// <summary>
    /// Tests that an invalid sale cancellation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale id When cancelling sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CancelSaleCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
      .Returns(new ValidationResult(new List<ValidationFailure>
      {
                new("Id", ValidationMessages.SaleIdRequired),
                new("CancellationReason", ValidationMessages.CancellationReasonRequired),
                new("CancellationReason", ValidationMessages.CancellationReasonMaxLength),
      }));


        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        var exception = await act.Should()
           .ThrowAsync<ValidationException>();

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "Id" && e.ErrorMessage == ValidationMessages.SaleIdRequired);

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "CancellationReason" && e.ErrorMessage == ValidationMessages.CancellationReasonRequired);

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "CancellationReason" && e.ErrorMessage == ValidationMessages.CancellationReasonMaxLength);
    }

    /// <summary>
    /// Tests that cancelling a non-existent sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale id When cancelling sale Then throws exception")]
    public async Task Handle_NonExistentSale_ThrowsException()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCancelSaleCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Sale)null);

        // When
        // When
        var cancelItemResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        cancelItemResult.Should().NotBeNull();
        cancelItemResult.Success.Should().BeFalse();
        cancelItemResult.Message.Should().Be(string.Format(ApiMessages.SaleNotFound, command.Id));
    }

    /// <summary>
    /// Tests that cancelling an already cancelled sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Given already cancelled sale When cancelling sale Then throws exception")]
    public async Task Handle_AlreadyCancelledSale_ThrowsException()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCancelSaleCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());

        var sale = new Sale
        {
            Id = command.Id,
            SaleNumber = "123",
            Customer = "John Doe",
            Branch = "Branch 1",
            IsCancelled = true,
            Items = new List<SaleItem>()
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        // When 
        var cancelItemResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        cancelItemResult.Should().NotBeNull();
        cancelItemResult.Success.Should().BeFalse();
        cancelItemResult.Message.Should().Be(ApiMessages.SaleAlreadyCancelled);
    }


}
