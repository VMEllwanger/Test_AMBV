using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
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
/// Contains unit tests for the <see cref="UpdateSaleHandler"/> class.
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly UpdateSaleHandler _handler;
    private readonly ISaleEventPublisher _eventPublisher;
    private readonly IValidator<UpdateSaleCommand> _validator;
    private readonly ILogger<UpdateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventPublisher = Substitute.For<ISaleEventPublisher>();
        _validator = Substitute.For<IValidator<UpdateSaleCommand>>();
        _logger = Substitute.For<ILogger<UpdateSaleHandler>>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper, _validator, _logger, _eventPublisher);
    }

    /// <summary>
    /// Tests that a valid sale update request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When updating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidUpdateSaleCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());

        var sale = new Sale
        {
            Id = command.Id,
            Customer = command.Customer,
            Branch = command.Branch,
            Items = command.Items.Select(i => new SaleItem
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        var updateSaleResult = new UpdateSaleResult
        {
            Id = sale.Id,
            Customer = sale.Customer,
            Branch = sale.Branch,
            Items = sale.Items.Select(i => new UpdateSaleItemResult
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        _mapper.Map<UpdateSaleResult>(sale).Returns(updateSaleResult);

        // When 
        var update = await _handler.Handle(command, CancellationToken.None);

        // Then
        update.Should().NotBeNull();
        update.Id.Should().Be(updateSaleResult.Id);
        update.Customer.Should().Be(updateSaleResult.Customer);
        update.Branch.Should().Be(updateSaleResult.Branch);
        update.Items.Should().HaveCount(updateSaleResult.Items.Count);

        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid sale update request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When updating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new UpdateSaleCommand();
        command.Items = new List<UpdateSaleItemCommand>();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new List<ValidationFailure>
            {
                new("ProductId", ValidationMessages.ProductIdRequired),
                new("ProductName", ValidationMessages.ProductNameRequired),
                new("Quantity", ValidationMessages.QuantityGreaterThanZero),
                new("UnitPrice",  ValidationMessages.UnitPriceGreaterThanZero),
            }));


        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        var exception = await act.Should()
           .ThrowAsync<ValidationException>();

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "ProductId" && e.ErrorMessage == ValidationMessages.ProductIdRequired);

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "ProductName" && e.ErrorMessage == ValidationMessages.ProductNameRequired);

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "Quantity" && e.ErrorMessage == ValidationMessages.QuantityGreaterThanZero);

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "UnitPrice" && e.ErrorMessage == ValidationMessages.UnitPriceGreaterThanZero);
    }

    /// <summary>
    /// Tests that the mapper is called with the correct command.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale id When updating sale Then throws exception")]
    public async Task andle_NonExistentSale_ThrowsException()
    {
        // Given
        // Given
        var command = SaleHandlerTestData.GenerateValidUpdateSaleCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Sale)null);

        // When 
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>();

    }

    /// <summary>
    /// Tests that cancelling an already cancelled sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Given an already cancelled sale, when attempting to update it, then an exception is thrown.")]
    public async Task Handle_AlreadyCancelledSaleUpdate_ThrowsException()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidUpdateSaleCommand();
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
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        var exception = await act.Should().ThrowAsync<InvalidOperationException>();
        exception.Which.Message.Should().Be(ApiMessages.CannotUpdateCancelledSale);
    }


    /// <summary>
    /// Tests that updating a non-existent sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale When updating Then throws exception")]
    public async Task Handle_NonExistentSale_ThrowsException()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidUpdateSaleCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Sale)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        var exception = await act.Should().ThrowAsync<KeyNotFoundException>();
        exception.Which.Message.Should().Be(string.Format(ApiMessages.SaleNotFound, command.Id));


    }
}
