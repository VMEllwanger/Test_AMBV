using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
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
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CreateSaleHandler _handler;
    private readonly ISaleEventPublisher _eventPublisher;
    private readonly IValidator<CreateSaleCommand> _validator;
    private readonly ILogger<CreateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventPublisher = Substitute.For<ISaleEventPublisher>();
        _validator = Substitute.For<IValidator<CreateSaleCommand>>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _validator, _logger, _eventPublisher);
    }

    /// <summary>
    /// Tests that a valid sale creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCreateSaleCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());

        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            Customer = command.Customer,
            Branch = command.Branch,
            Items = command.Items.Select(i => new SaleItem
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        var result = new CreateSaleResult
        {
            Id = sale.Id
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleResult.Should().NotBeNull();
        createSaleResult.Id.Should().Be(sale.Id);
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid sale creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid saleItems data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidSaleItemsRequest_ThrowsValidationException()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCreateSaleCommand();
        command.Items = new List<CreateSaleItemCommand>();
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
    /// Tests that an invalid sale creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateSaleCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new List<ValidationFailure>
            {
                new("SaleNumber", ValidationMessages.SaleNumberRequired),
                new("Date", ValidationMessages.SaleDateRequired),
                new("Customer", ValidationMessages.CustomerRequired),
                new("Branch",  ValidationMessages.BranchRequired),
                new("Items", ValidationMessages.SaleItemsRequired)
            }));
        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        var exception = await act.Should()
           .ThrowAsync<FluentValidation.ValidationException>();

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "SaleNumber" && e.ErrorMessage == ValidationMessages.SaleNumberRequired);

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "Date" && e.ErrorMessage == ValidationMessages.SaleDateRequired);

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "Customer" && e.ErrorMessage == ValidationMessages.CustomerRequired);

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "Branch" && e.ErrorMessage == ValidationMessages.BranchRequired);

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "Items" && e.ErrorMessage == ValidationMessages.SaleItemsRequired);

    }


    /// <summary>
    /// Tests that the mapper is called with the correct command.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps command to sale entity")]
    public async Task Handle_ValidRequest_MapsCommandToSale()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCreateSaleCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());


        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            Customer = command.Customer,
            Branch = command.Branch,
            Items = command.Items.Select(i => new SaleItem
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<Sale>(Arg.Is<CreateSaleCommand>(c =>
            c.SaleNumber == command.SaleNumber &&
            c.Customer == command.Customer &&
            c.Branch == command.Branch &&
            c.Items.Count == command.Items.Count));
    }

    /// <summary>
    /// Tests that the sale items are correctly mapped and saved.
    /// </summary>
    [Fact(DisplayName = "Given valid command with items When handling Then saves sale with correct items")]
    public async Task Handle_ValidRequestWithItems_SavesSaleWithCorrectItems()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCreateSaleCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());


        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            Customer = command.Customer,
            Branch = command.Branch,
            Items = command.Items.Select(i => new SaleItem
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s =>
                s.Items.Count == command.Items.Count &&
                s.Items.All(i =>
                    command.Items.Any(ci =>
                        ci.ProductId == i.ProductId &&
                        ci.ProductName == i.ProductName &&
                        ci.Quantity == i.Quantity &&
                        ci.UnitPrice == i.UnitPrice))),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that discounts are applied correctly based on item quantity when creating a sale.
    /// </summary>
    [Theory]
    [InlineData(3, 0.0)]
    [InlineData(4, 0.10)]
    [InlineData(9, 0.10)]
    [InlineData(10, 0.20)]
    [InlineData(20, 0.20)]
    public async Task Handle_WhenCreatingSale_AppliesCorrectDiscount(int quantity, decimal expectedDiscount)
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCreateSaleCommand();
        command.Items = new List<CreateSaleItemCommand>
        {
            new()
            {
                ProductId = "1",
                ProductName = "Product 1",
                Quantity = quantity,
                UnitPrice = 100
            }
        };
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());

        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            Customer = command.Customer,
            Branch = command.Branch,
            Items = command.Items.Select(i => new SaleItem
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = 0
            }).ToList()
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s =>
                s.Items.Count == 1 &&
                s.Items[0].Quantity == quantity &&
                s.Items[0].Discount == expectedDiscount),
            Arg.Any<CancellationToken>());
    }
}
