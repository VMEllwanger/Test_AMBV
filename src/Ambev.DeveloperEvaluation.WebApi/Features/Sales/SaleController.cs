using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
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
public class SaleController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly IMapper _mapper;

  /// <summary>
  /// Initializes a new instance of SaleController
  /// </summary>
  /// <param name="mediator">The mediator instance</param>
  /// <param name="mapper">The AutoMapper instance</param> 
  public SaleController(IMediator mediator, IMapper mapper)
  {
    _mediator = mediator;
    _mapper = mapper;
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
    var validator = new CreateSaleRequestValidator();
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid)
      return BadRequest(validationResult.Errors);

    var command = _mapper.Map<CreateSaleCommand>(request);

    var response = await _mediator.Send(command);


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
    var request = new GetSaleRequest { Id = id };

    var validator = new GetSaleRequestValidator();
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid)
      return BadRequest(validationResult.Errors);

    var command = _mapper.Map<GetSaleCommand>(request.Id);
    var response = await _mediator.Send(command);

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
    var validator = new GetAllSaleRequestValidator();
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid)
      return BadRequest(validationResult.Errors);


    var command = _mapper.Map<GetAllSaleCommand>(request);

    var response = await _mediator.Send(command);
    return Ok(new ApiResponseWithData<GetAllSaleResponse>
    {
      Success = true,
      Message = "Sale retrieved successfully",
      Data = _mapper.Map<GetAllSaleResponse>(response)
    });
  }

  /// <summary>
  /// Retrieves a sale by their ID
  /// </summary>
  /// <param name="id">The unique identifier of the sale</param>
  /// <param name="cancellationToken">Cancellation token</param>
  /// <returns>The sale details if found</returns>
  [HttpPut("{id}")]
  [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
  public async Task<ActionResult<UpdateSaleResponse>> Update(Guid id, CancellationToken cancellationToken)
  {
    var request = new UpdateSaleRequest { Id = id };
    var validator = new UpdateSaleRequestValidator();
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid)
      return BadRequest(validationResult.Errors);

    var command = _mapper.Map<UpdateSaleCommand>(request);

    var result = await _mediator.Send(command);


    return Ok(new ApiResponseWithData<UpdateSaleResponse>
    {
      Success = true,
      Message = "Update Sale retrieved successfully",
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
    var request = new DeleteSaleRequest { Id = id };
    var validator = new DeleteSaleRequestValidator();
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid)
      return BadRequest(validationResult.Errors);

    var command = _mapper.Map<DeleteSaleCommand>(request.Id);
    await _mediator.Send(command, cancellationToken);

    return Ok(new ApiResponse
    {
      Success = true,
      Message = "Sale deleted successfully"
    });
  }
}
