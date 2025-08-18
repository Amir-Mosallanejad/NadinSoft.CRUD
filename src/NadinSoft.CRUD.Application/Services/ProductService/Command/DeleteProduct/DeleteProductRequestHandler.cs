using MediatR;
using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.DeleteProduct;

/// <summary>
/// Handles requests to delete a <see cref="Product"/> entity.
/// Ensures the requesting user is authorized and the product exists before deletion.
/// </summary>
public class DeleteProductRequestHandler(
    IProductRepository productRepository,
    ICurrentUserService currentUserService,
    ILogger<DeleteProductRequestHandler> logger,
    ILocalizationService localizationService)
    : IRequestHandler<DeleteProductRequest, ApiResponse<object>>
{
    /// <summary>
    /// Handles the <see cref="DeleteProductRequest"/> by validating the user,
    /// verifying ownership, and removing the product from the repository.
    /// </summary>
    /// <param name="request">The request containing the product ID to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// An <see cref="ApiResponse{T}"/> indicating success if the product was deleted,
    /// or failure if the user is unauthorized or the product does not exist.
    /// </returns>
    public async Task<ApiResponse<object>> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            string? userId = currentUserService.UserId;
            if (userId is null)
            {
                return ApiResponse<object>.Fail(
                    localizationService.GetApiMessageResource(ApiMessageResourceKey.UserUnauthorized));
            }

            Product? product = await productRepository.GetByIdAsync(request.ProductId);

            if (product is null)
            {
                logger.ProductNotFoundLogger(request.ProductId);
                return ApiResponse<object>.Fail(
                    localizationService.GetApiMessageResource(ApiMessageResourceKey.ProductNotFound));
            }

            if (product.CreatedByUserId != userId)
            {
                logger.UnauthorizedDeleteAttemptLogger(userId, product.Id, product.CreatedByUserId);

                return ApiResponse<object>.Fail(
                    localizationService.GetApiMessageResource(ApiMessageResourceKey.NotOwnerOfProductDelete));
            }

            productRepository.Remove(product);

            return ApiResponse<object>.Success(new object());
        }
        catch (Exception exception)
        {
            logger.UnhandledErrorLogger(exception);

            return ApiResponse<object>.Fail(
                localizationService.GetApiMessageResource(ApiMessageResourceKey.UnexpectedError));
        }
    }
}