using Ambev.DeveloperEvaluation.Application.Sales.CancelItem;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelItemSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing sales
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<SalesController> _logger;

    /// <summary>
    /// Initializes a new instance of SaleController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param> 
    public SalesController(IMediator mediator, IMapper mapper, ILogger<SalesController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new sale
    /// </summary>
    /// <param name="request">The request containing sale details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    [HttpPost]
    public async Task<ActionResult<CreateSaleResponse>> Create([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new sale for customer: {Customer}", request.Customer);

        var validator = new CreateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Invalid sale creation request: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<CreateSaleCommand>(request);
        var response = await _mediator.Send(command);

        _logger.LogInformation("Sale created successfully with ID: {SaleId}", response.Id);

        return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
        {
            Success = true,
            Message = "Sale created successfully",
            Data = _mapper.Map<CreateSaleResponse>(response)
        });
    }

    /// <summary>
    /// Retrieves a sale by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale details if found</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetSaleResponse>> Get(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving sale with ID: {SaleId}", id);

        var request = new GetSaleRequest { Id = id };
        var validator = new GetSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Invalid sale retrieval request: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<GetSaleCommand>(request.Id);
        var response = await _mediator.Send(command);

        _logger.LogInformation("Sale retrieved successfully: {SaleId}", id);

        return Ok(new ApiResponseWithData<GetSaleResponse>
        {
            Success = true,
            Message = "Sale retrieved successfully",
            Data = _mapper.Map<GetSaleResponse>(response)
        });
    }

    /// <summary>
    /// Retrieves all sales with pagination
    /// </summary>
    /// <param name="request">The request containing pagination and filtering parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated list of sales</returns>
    [HttpGet]
    public async Task<ActionResult<GetAllSaleResponse>> GetAll([FromQuery] GetAllSaleRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving sales list with filters: {@Filters}", new { request.Page, request.PageSize, request.SearchTerm, request.IsCancelled });

        var validator = new GetAllSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Invalid sales list request: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<GetAllSaleCommand>(request);
        var response = await _mediator.Send(command);

        _logger.LogInformation("Sales list retrieved successfully. Total items: {TotalCount}", response.TotalItems);

        return Ok(new ApiResponseWithData<GetAllSaleResponse>
        {
            Success = true,
            Message = "Sale retrieved successfully",
            Data = _mapper.Map<GetAllSaleResponse>(response)
        });
    }

    /// <summary>
    /// Updates a sale by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="request">The sale data to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateSaleResponse>> Update(Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
    {
        request.Id = id;
        var validator = new UpdateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateSaleCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<UpdateSaleResponse>
        {
            Success = true,
            Message = "Sale updated successfully",
            Data = _mapper.Map<UpdateSaleResponse>(result)
        });
    }

    /// <summary>
    /// Deletes a sale by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the sale was deleted</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteSaleResponse>> Delete(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting sale with ID: {SaleId}", id);

        var request = new DeleteSaleRequest { Id = id };
        var validator = new DeleteSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Invalid sale deletion request: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<DeleteSaleCommand>(request);
        await _mediator.Send(command, cancellationToken);

        _logger.LogInformation("Sale deleted successfully: {SaleId}", id);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Sale deleted successfully"
        });
    }

    /// <summary>
    /// Cancels a sale by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale to cancel</param>
    /// <param name="request">The cancellation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the sale was cancelled</returns>
    [HttpPost("{id}/cancel")]
    public async Task<ActionResult<ApiResponse>> CancelSale(Guid id, [FromBody] CancelSaleRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving sale with ID: {SaleId}", id);

        request.Id = id;
        var validator = new CancelSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Invalid sale retrieval request: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<CancelSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<CancelSaleResponse>
        {
            Success = true,
            Message = "Sale cancelled successfully",
        });
    }

    /// <summary>
    /// Cancels an item by their ID
    /// </summary>
    /// <param name="saleId">The unique identifier of the sale</param>
    /// <param name="itemId">The unique identifier of the item to cancel</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the item was cancelled</returns>
    [HttpPost("{saleId}/items/{itemId}/cancel")]
    public async Task<ActionResult<ApiResponse>> CancelItem(Guid saleId, Guid itemId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cancelling item {ItemId} from sale {SaleId}", itemId, saleId);

        var request = new CancelItemSaleRequest
        {
            SaleId = saleId,
            Id = itemId
        };
        var validator = new CancelItemSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Invalid sale retrieval request: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<CancelItemCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Item cancelled successfully"
        });
    }
}