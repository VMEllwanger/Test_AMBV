using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Constants;
using Ambev.DeveloperEvaluation.Domain.Entities;
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
/// Contains unit tests for the <see cref="GetSaleHandler"/> class.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleHandler _handler;
    private readonly IValidator<GetSaleCommand> _validator;
    private readonly ILogger<GetSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _validator = Substitute.For<IValidator<GetSaleCommand>>();
        _logger = Substitute.For<ILogger<GetSaleHandler>>();
        _handler = new GetSaleHandler(_saleRepository, _mapper, _validator, _logger);
    }

    /// <summary>
    /// Tests that a valid sale retrieval request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale id When getting sale Then returns sale data")]
    public async Task Handle_ValidRequest_ReturnsSaleData()
    {
        // Given
        var command = new GetSaleCommand { Id = Guid.NewGuid() };
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

        var result = new GetSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            Customer = sale.Customer,
            Branch = sale.Branch,
            Items = sale.Items.Select(i => new GetSaleItemResult
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount
            }).ToList()
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(result);

        // When
        var getSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getSaleResult.Should().NotBeNull();
        getSaleResult.Id.Should().Be(sale.Id);
        getSaleResult.SaleNumber.Should().Be(sale.SaleNumber);
        getSaleResult.Customer.Should().Be(sale.Customer);
        getSaleResult.Branch.Should().Be(sale.Branch);
        getSaleResult.Items.Should().HaveCount(1);
        getSaleResult.Items[0].ProductId.Should().Be(sale.Items[0].ProductId);
        getSaleResult.Items[0].ProductName.Should().Be(sale.Items[0].ProductName);
        getSaleResult.Items[0].Quantity.Should().Be(sale.Items[0].Quantity);
        getSaleResult.Items[0].UnitPrice.Should().Be(sale.Items[0].UnitPrice);
        getSaleResult.Items[0].Discount.Should().Be(sale.Items[0].Discount);

    }

    /// <summary>
    /// Tests that an invalid sale retrieval request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale id When getting sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new GetSaleCommand();
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
    /// Tests that retrieving a non-existent sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale id When getting sale Then throws exception")]
    public async Task Handle_NonExistentSale_ThrowsException()
    {
        // Given
        var command = new GetSaleCommand { Id = Guid.NewGuid() };
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Sale)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        var exception = await act.Should()
            .ThrowAsync<KeyNotFoundException>();

        exception.Which.Message.Should().Be(string.Format(ApiMessages.SaleNotFound, command.Id));
    }
}
