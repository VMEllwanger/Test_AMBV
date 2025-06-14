using Ambev.DeveloperEvaluation.Application.Sales.CancelItem;
using Ambev.DeveloperEvaluation.Domain.Constants;
using Ambev.DeveloperEvaluation.Domain.Entities;
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
/// Contains unit tests for the <see cref="CancelItemHandler"/> class.
/// </summary>
public class CancelItemHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CancelItemHandler _handler;
    private readonly ISaleEventPublisher _eventPublisher;
    private readonly IValidator<CancelItemCommand> _validator;
    private readonly ILogger<CancelItemHandler> _logger;



    /// <summary>
    /// Initializes a new instance of the <see cref="CancelItemHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CancelItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventPublisher = Substitute.For<ISaleEventPublisher>();
        _validator = Substitute.For<IValidator<CancelItemCommand>>();
        _logger = Substitute.For<ILogger<CancelItemHandler>>();
        _handler = new CancelItemHandler(_saleRepository, _mapper, _validator, _logger, _eventPublisher);
    }

    /// <summary>
    /// Tests that a valid item cancellation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid item id When cancelling item Then returns success")]
    public async Task Handle_ValidRequest_ReturnsSuccess()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCancelItemCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());

        var sale = new Sale
        {
            Id = command.SaleId,
            SaleNumber = "123",
            Customer = "John Doe",
            Branch = "Branch 1",
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = command.ItemId,
                    ProductId = "1",
                    ProductName = "Product 1",
                    Quantity = 1,
                    UnitPrice = 100,
                    Discount = 0
                }
            }
        };

        var result = new CancelItemResult
        {
            Success = true,
            Message = ApiMessages.ItemCancelled
        };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<CancelItemResult>(Arg.Any<SaleItem>()).Returns(result);

        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        var cancelItemResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        cancelItemResult.Should().NotBeNull();
        cancelItemResult.Success.Should().BeTrue();
        cancelItemResult.Message.Should().Be(ApiMessages.ItemCancelled);
        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<Sale>(s =>
                s.Id == sale.Id &&
                s.Items.Any(i => i.Id == command.ItemId && i.IsCancelled)),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid item cancellation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid item id When cancelling item Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given 
        var command = new CancelItemCommand();

        // Adiciona à lista de erros
        var _errors = new List<ValidationFailure> {
            new ValidationFailure("SaleId", ValidationMessages.SaleIdRequired),
            new ValidationFailure("ItemId", ValidationMessages.ItemIdRequired)
        };

        // Simula retorno de validação com erro
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
                  .Returns(new ValidationResult(_errors));

        // When 
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then - 
        var exception = await act.Should()
            .ThrowAsync<ValidationException>();

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "SaleId" && e.ErrorMessage == ValidationMessages.SaleIdRequired);

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "ItemId" && e.ErrorMessage == ValidationMessages.ItemIdRequired);
    }

    /// <summary>
    /// Tests that cancelling an item from a non-existent sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale id When cancelling item Then throws exception")]
    public async Task Handle_NonExistentSale()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCancelItemCommand();

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns((Sale)null);


        // When
        var cancelItemResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        cancelItemResult.Should().NotBeNull();
        cancelItemResult.Success.Should().BeFalse();
        cancelItemResult.Message.Should().Be(string.Format(ApiMessages.SaleNotFound, command.SaleId));
    }



    /// <summary>
    /// Tests that cancelling a non-existent item throws an exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent item id When cancelling item")]
    public async Task Handle_NonExistentItem()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCancelItemCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());
        var sale = new Sale
        {
            Id = command.SaleId,
            SaleNumber = "123",
            Customer = "John Doe",
            Branch = "Branch 1",
            Items = new List<SaleItem>()
        };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // When 
        var cancelItemResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        cancelItemResult.Should().NotBeNull();
        cancelItemResult.Success.Should().BeFalse();
        cancelItemResult.Message.Should().Be(ApiMessages.ItemNotFoundInSale);
    }

    /// <summary>
    /// Tests that GetById returns the item as cancelled.
    /// </summary>
    [Fact(DisplayName = "Given cancelled item When getting by id Then returns cancelled item")]
    public async Task GetById_ReturnsCancelledItem()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCancelItemCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());

        var sale = new Sale
        {
            Id = command.SaleId,
            SaleNumber = "123",
            Customer = "John Doe",
            Branch = "Branch 1",
            IsCancelled = false,
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = command.ItemId,
                    ProductId = "1",
                    ProductName = "Product 1",
                    Quantity = 1,
                    UnitPrice = 100,
                    Discount = 0,
                    IsCancelled = true
                }
            }
        };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);



        // When 
        var cancelItemResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        cancelItemResult.Should().NotBeNull();
        cancelItemResult.Success.Should().BeFalse();
        cancelItemResult.Message.Should().Be(ApiMessages.ItemAlreadyCancelled);
    }

    /// <summary>
    /// Tests that GetById returns the item as cancelled.
    /// </summary>
    [Fact(DisplayName = "Given cancelled sale When getting by id Then returns sale cancelled")]
    public async Task GetById_ReturnsSaleCancelledItem()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCancelItemCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());

        var sale = new Sale
        {
            Id = command.SaleId,
            SaleNumber = "123",
            Customer = "John Doe",
            Branch = "Branch 1",
            IsCancelled = true,
            Items = new List<SaleItem> { }
        };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // When 
        var cancelItemResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        cancelItemResult.Should().NotBeNull();
        cancelItemResult.Success.Should().BeFalse();
        cancelItemResult.Message.Should().Be(ApiMessages.CannotCancelItemFromCancelledSale);
    }
}
