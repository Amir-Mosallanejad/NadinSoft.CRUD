using MediatR;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Services.ProductService.DTOs;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Application.Services.ProductService.Query.GetAllProducts;

/// <summary>
/// Represents a request to retrieve a paginated list of <see cref="Product"/> entities,
/// optionally filtered by name.
/// </summary>
/// <remarks>
/// Inherits from <see cref="PaginatedRequest"/> to support pagination parameters
/// such as <see cref="PaginatedRequest.Page"/> and <see cref="PaginatedRequest.PerPage"/>.
/// Implements <see cref="IRequest{TResponse}"/> with a response type of
/// <see cref="ApiResponse{T}"/> containing a <see cref="PaginatedResponse{T}"/> of <see cref="ProductResponseDto"/>.
/// </remarks>
public class GetAllProductsRequest : PaginatedRequest, IRequest<ApiResponse<PaginatedResponse<ProductResponseDto>>>
{
    /// <summary>
    /// Gets or sets optional filter to search products by name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}