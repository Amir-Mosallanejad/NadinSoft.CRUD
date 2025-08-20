using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;
using NadinSoft.CRUD.Application.Services.ProductService.DTOs;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;
using System.Linq.Expressions;

namespace NadinSoft.CRUD.Application.Services.ProductService.Query.GetAllProducts;

/// <summary>
/// Handles requests to retrieve a paginated list of <see cref="Product"/> entities,
/// optionally filtered by name.
/// </summary>
public class GetAllProductsRequestHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<GetAllProductsRequestHandler> logger,
    ILocalizationService localizationService)
    : IRequestHandler<GetAllProductsRequest, ApiResponse<PaginatedResponse<ProductResponseDto>>>
{
    /// <summary>
    /// Handles the <see cref="GetAllProductsRequest"/> by querying the repository,
    /// mapping the results to <see cref="ProductResponseDto"/>, and returning a paginated response.
    /// </summary>
    /// <param name="request">The request containing pagination parameters and optional name filter.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// An <see cref="ApiResponse{T}"/> containing a <see cref="PaginatedResponse{T}"/> of <see cref="ProductResponseDto"/>
    /// with the filtered and paginated products, or a failure response if an error occurs.
    /// </returns>
    public async Task<ApiResponse<PaginatedResponse<ProductResponseDto>>> Handle(
        GetAllProductsRequest request,
        CancellationToken cancellationToken)
    {
        ApiResponse<PaginatedResponse<ProductResponseDto>> response;

        try
        {
            IBaseRepository<Product> productRepo = unitOfWork.GetRepository<Product>();

            Expression<Func<Product, bool>> filter = x => x.Name.Contains(request.Name);

            (int Total, IEnumerable<Product> Items) products = await productRepo.GetByFiltersAsync(
                filter,
                request.Page,
                request.PerPage);

            List<ProductResponseDto> data = products.Items.Select(mapper.Map<ProductResponseDto>).ToList();

            response = ApiResponse<PaginatedResponse<ProductResponseDto>>.Success(
                new PaginatedResponse<ProductResponseDto>
                {
                    Items = data,
                    Page = request.Page,
                    PerPage = request.PerPage,
                    TotalCount = products.Total,
                });
        }
        catch (Exception exception)
        {
            logger.UnhandledErrorLogger(exception);

            return ApiResponse<PaginatedResponse<ProductResponseDto>>.Fail(
                localizationService.GetApiMessageResource(ApiMessageResourceKey.UnexpectedError));
        }

        return response;
    }
}