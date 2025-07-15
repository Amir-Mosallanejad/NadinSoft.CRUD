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

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<ApiResponse<object>> Create([FromBody] CreateProductRequestDto request)
    {
        return await _mediator.Send(new CreateProductRequest(request));
    }

    [Authorize]
    [HttpPut("update")]
    public async Task<ApiResponse<object>> Update([FromBody] UpdateProductRequestDto request)
    {
        return await _mediator.Send(new UpdateProductRequest(request));
    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<ApiResponse<object>> Delete(Guid productId)
    {
        return await _mediator.Send(new DeleteProductRequest(productId));
    }

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<PaginatedResponse<ProductResponseDto>>> GetAll(
        [FromQuery] GetAllProductsRequest request)
    {
        return await _mediator.Send(request);
    }
}