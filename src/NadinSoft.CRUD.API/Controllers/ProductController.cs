using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;
using NadinSoft.CRUD.Application.Services.ProductService.Command.DeleteProduct;
using NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;
using NadinSoft.CRUD.Application.Services.ProductService.DTOs;
using NadinSoft.CRUD.Application.Services.ProductService.Query.GetAllProducts;

namespace NadinSoft.CRUD.API.Controllers;

/// <summary>
/// Provides product management endpoints for creating, updating, deleting, and retrieving products.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    /// <summary>
    /// Sends requests to handlers and mediates communication between application components.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator used to send product-related requests.</param>
    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="request">The product creation data transfer object.</param>
    /// <returns>
    /// An <see cref="ApiResponse{T}"/> indicating success or failure of the product creation.
    /// </returns>
    [Authorize]
    [HttpPost("create")]
    public async Task<ApiResponse<object>> Create([FromBody] CreateProductRequestDto request)
    {
        return await _mediator.Send(new CreateProductRequest(request));
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="request">The product update data transfer object.</param>
    /// <returns>
    /// An <see cref="ApiResponse{T}"/> indicating success or failure of the product update.
    /// </returns>
    [Authorize]
    [HttpPut("update")]
    public async Task<ApiResponse<object>> Update([FromBody] UpdateProductRequestDto request)
    {
        return await _mediator.Send(new UpdateProductRequest(request));
    }

    /// <summary>
    /// Deletes a product by its unique identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to delete.</param>
    /// <returns>
    /// An <see cref="ApiResponse{T}"/> indicating success or failure of the product deletion.
    /// </returns>
    [Authorize]
    [HttpDelete("delete")]
    public async Task<ApiResponse<object>> Delete(Guid productId)
    {
        return await _mediator.Send(new DeleteProductRequest(productId));
    }

    /// <summary>
    /// Retrieves all products with pagination and optional filters.
    /// </summary>
    /// <param name="request">The request object containing pagination and filter parameters.</param>
    /// <returns>
    /// An <see cref="ApiResponse{T}"/> containing a paginated list of <see cref="ProductResponseDto"/> objects.
    /// </returns>
    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<PaginatedResponse<ProductResponseDto>>> GetAll(
        [FromQuery] GetAllProductsRequest request)
    {
        return await _mediator.Send(request);
    }
}