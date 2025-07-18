using MediatR;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Services.ProductService.DTOs;

namespace NadinSoft.CRUD.Application.Services.ProductService.Query.GetAllProducts;

public class GetAllProductsRequest : PaginatedRequest, IRequest<ApiResponse<PaginatedResponse<ProductResponseDto>>>
{
    public string Name { get; set; } = string.Empty;
}