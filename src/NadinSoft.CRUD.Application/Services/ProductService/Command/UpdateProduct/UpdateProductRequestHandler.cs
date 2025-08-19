using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;

/// <summary>
/// Handles requests to update an existing <see cref="Product"/> entity.
/// Ensures the requesting user is authorized and the product exists before applying updates.
/// </summary>
public class UpdateProductRequestHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IMapper mapper,
    ILogger<UpdateProductRequestHandler> logger,
    ILocalizationService localizationService)
    : IRequestHandler<UpdateProductRequest, ApiResponse<object>>
{
    /// <summary>
    /// Handles the <see cref="UpdateProductRequest"/> by validating the user,
    /// verifying ownership, mapping updated values, and saving changes to the repository.
    /// </summary>
    /// <param name="request">The request containing the product update data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// An <see cref="ApiResponse{T}"/> indicating success if the product was updated,
    /// or failure if the user is unauthorized or the product does not exist.
    /// </returns>
    public async Task<ApiResponse<object>> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            string? userId = currentUserService.UserId;
            if (userId is null)
            {
                return ApiResponse<object>.Fail(
                    localizationService.GetApiMessageResource(ApiMessageResourceKey.UserUnauthorized));
            }

            Product? product = await unitOfWork.ProductRepository.GetByIdAsync(request.Dto.Id);

            if (product is null)
            {
                logger.ProductNotFoundLogger(request.Dto.Id);

                return ApiResponse<object>.Fail(
                    localizationService.GetApiMessageResource(ApiMessageResourceKey.ProductNotFound));
            }

            if (product.CreatedByUserId != userId)
            {
                logger.UnauthorizedUpdateAttemptLogger(userId, product.Id, product.CreatedByUserId);

                return ApiResponse<object>.Fail(
                    localizationService.GetApiMessageResource(ApiMessageResourceKey.NotOwnerOfProductUpdate));
            }

            mapper.Map(request.Dto, product);
            product.CreatedByUserId = userId;

            unitOfWork.ProductRepository.Update(product);
            await unitOfWork.SaveChangesAsync();

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