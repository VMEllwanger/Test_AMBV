using Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;
using Ambev.DeveloperEvaluation.Domain.Common;
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
/// Contains unit tests for the <see cref="GetAllSaleHandler"/> class.
/// </summary>
public class GetAllSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetAllSaleHandler _handler;
    private readonly ISaleEventPublisher _eventPublisher;
    private readonly IValidator<GetAllSaleCommand> _validator;
    private readonly ILogger<GetAllSaleHandler> _logger;


    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetAllSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventPublisher = Substitute.For<ISaleEventPublisher>();
        _validator = Substitute.For<IValidator<GetAllSaleCommand>>();
        _logger = Substitute.For<ILogger<GetAllSaleHandler>>();
        _handler = new GetAllSaleHandler(_saleRepository, _mapper, _validator, _logger);
    }

    /// <summary>
    /// Tests that a valid sale list retrieval request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid pagination When getting sales Then returns sales list")]
    public async Task Handle_ValidRequest_ReturnsSalesList()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidGetAllSaleCommand();

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());
        var sales = new List<Sale>
        {
            new()
            {
                Id = Guid.NewGuid(),
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
            },
            new()
            {
                Id = Guid.NewGuid(),
                SaleNumber = "124",
                Customer = "Jane Doe",
                Branch = "Branch 2",
                Items = new List<SaleItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = "2",
                        ProductName = "Product 2",
                        Quantity = 2,
                        UnitPrice = 200,
                        Discount = 10
                    }
                }
            }
        };

        var paginatedResult = new PaginatedResult<Sale>
        {
            Items = sales,
            TotalCount = 2,
            Page = command.Page,
            PageSize = command.PageSize
        };

        var result = new GetAllSaleResult
        {
            Data = sales.Select(s => new GetAllSaleItemResult
            {
                Id = s.Id,
                SaleNumber = s.SaleNumber,
                Customer = s.Customer,
                Branch = s.Branch,
                TotalAmount = s.TotalAmount,
                IsCancelled = s.IsCancelled
            }).ToList(),
            TotalItems = 2,
            CurrentPage = command.Page,
            TotalPages = 1
        };

        _saleRepository.GetAllAsync(command.Page, command.PageSize, null, null, Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(paginatedResult);
        _mapper.Map<GetAllSaleResult>(paginatedResult).Returns(result);

        // When
        var getAllSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getAllSaleResult.Should().NotBeNull();
        getAllSaleResult.Data.Should().HaveCount(2);
        getAllSaleResult.TotalItems.Should().Be(2);
        getAllSaleResult.CurrentPage.Should().Be(command.Page);
        getAllSaleResult.TotalPages.Should().Be(1);

        getAllSaleResult.Data[0].Id.Should().Be(sales[0].Id);
        getAllSaleResult.Data[0].SaleNumber.Should().Be(sales[0].SaleNumber);
        getAllSaleResult.Data[0].Customer.Should().Be(sales[0].Customer);
        getAllSaleResult.Data[0].Branch.Should().Be(sales[0].Branch);
        getAllSaleResult.Data[0].TotalAmount.Should().Be(sales[0].TotalAmount);
        getAllSaleResult.Data[0].IsCancelled.Should().Be(sales[0].IsCancelled);

        getAllSaleResult.Data[1].Id.Should().Be(sales[1].Id);
        getAllSaleResult.Data[1].SaleNumber.Should().Be(sales[1].SaleNumber);
        getAllSaleResult.Data[1].Customer.Should().Be(sales[1].Customer);
        getAllSaleResult.Data[1].Branch.Should().Be(sales[1].Branch);
        getAllSaleResult.Data[1].TotalAmount.Should().Be(sales[1].TotalAmount);
        getAllSaleResult.Data[1].IsCancelled.Should().Be(sales[1].IsCancelled);
    }

    /// <summary>
    /// Tests that an invalid sale list retrieval request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid pagination When getting sales Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new GetAllSaleCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
                new("Page", ValidationMessages.PageGreaterThanZero),
                new("PageSize", ValidationMessages.PageSizeBetweenOneAndHundred),
                new("StartDate", ValidationMessages.StartDateLessThanEndDate),
                }));



        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        var exception = await act.Should()
           .ThrowAsync<ValidationException>();

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "Page" && e.ErrorMessage == ValidationMessages.PageGreaterThanZero);

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "PageSize" && e.ErrorMessage == ValidationMessages.PageSizeBetweenOneAndHundred);

        exception.Which.Errors.Should().Contain(e =>
            e.PropertyName == "StartDate" && e.ErrorMessage == ValidationMessages.StartDateLessThanEndDate);
    }

    /// <summary>
    /// Tests that retrieving an empty sale list returns an empty result.
    /// </summary>
    [Fact(DisplayName = "Given no sales When getting sales Then returns empty list")]
    public async Task Handle_NoSales_ReturnsEmptyList()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidGetAllSaleCommand();

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());
        var paginatedResult = new PaginatedResult<Sale>
        {
            Items = new List<Sale>(),
            TotalCount = 0,
            Page = command.Page,
            PageSize = command.PageSize
        };

        var result = new GetAllSaleResult
        {
            Data = new List<GetAllSaleItemResult>(),
            TotalItems = 0,
            CurrentPage = command.Page,
            TotalPages = 0
        };

        _saleRepository.GetAllAsync(command.Page, command.PageSize, null, null, Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(paginatedResult);
        _mapper.Map<GetAllSaleResult>(paginatedResult).Returns(result);

        // When
        var getAllSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getAllSaleResult.Should().NotBeNull();
        getAllSaleResult.Data.Should().BeEmpty();
        getAllSaleResult.TotalItems.Should().Be(0);
        getAllSaleResult.CurrentPage.Should().Be(command.Page);
        getAllSaleResult.TotalPages.Should().Be(0);
    }

    /// <summary>
    /// Tests that the mapper is called with the correct paginated result.
    /// </summary>
    [Fact(DisplayName = "Given valid paginated result When handling Then maps to result")]
    public async Task Handle_ValidPaginatedResult_MapsToResult()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidGetAllSaleCommand();

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());
        var sales = new List<Sale>
        {
            new()
            {
                Id = Guid.NewGuid(),
                SaleNumber = "123",
                Customer = "John Doe",
                Branch = "Branch 1",
                Items = new List<SaleItem>()
            }
        };

        var paginatedResult = new PaginatedResult<Sale>
        {
            Items = sales,
            TotalCount = 1,
            Page = command.Page,
            PageSize = command.PageSize
        };

        var result = new GetAllSaleResult
        {
            Data = sales.Select(s => new GetAllSaleItemResult
            {
                Id = s.Id,
                SaleNumber = s.SaleNumber,
                Customer = s.Customer,
                Branch = s.Branch,
                TotalAmount = s.TotalAmount,
                IsCancelled = s.IsCancelled
            }).ToList(),
            TotalItems = 1,
            CurrentPage = command.Page,
            TotalPages = 1
        };

        _saleRepository.GetAllAsync(command.Page, command.PageSize, null, null, Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(paginatedResult);
        _mapper.Map<GetAllSaleResult>(paginatedResult).Returns(result);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<GetAllSaleResult>(Arg.Is<PaginatedResult<Sale>>(p =>
            p.Items.Count == paginatedResult.Items.Count &&
            p.TotalCount == paginatedResult.TotalCount &&
            p.Page == paginatedResult.Page &&
            p.PageSize == paginatedResult.PageSize));
    }
}
