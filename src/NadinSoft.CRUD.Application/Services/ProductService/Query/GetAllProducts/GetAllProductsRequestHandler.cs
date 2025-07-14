using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Services.ProductService.DTOs;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

namespace NadinSoft.CRUD.Application.Services.ProductService.Query.GetAllProducts;

public class GetAllProductsRequestHandler(
    IProductRepository productRepository,
    IMapper mapper,
    ILogger<GetAllProductsRequestHandler> logger)
    : IRequestHandler<GetAllProductsRequest, ApiResponse<PaginatedResponse<ProductResponseDto>>>

{
    public async Task<ApiResponse<PaginatedResponse<ProductResponseDto>>> Handle(GetAllProductsRequest request,
        CancellationToken cancellationToken)
    {
        ApiResponse<PaginatedResponse<ProductResponseDto>> response;

        try
        {
            (int Total, IEnumerable<Product> Items) customers = await productRepository.GetProductsByFilters(
                request.Name.ToLower(),
                request.Page,
                request.PerPage);

            List<ProductResponseDto> data = customers.Items.Select(mapper.Map<ProductResponseDto>).ToList();

            response = ApiResponse<PaginatedResponse<ProductResponseDto>>.Success(
                new PaginatedResponse<ProductResponseDto>
                {
                    Items = data,
                    Page = request.Page,
                    PerPage = request.PerPage,
                    TotalCount = customers.Total,
                });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while retrieving all products.");
            return ApiResponse<PaginatedResponse<ProductResponseDto>>.Fail(
                "An unexpected error occurred while retrieving products.");
        }

        return response;
    }
}