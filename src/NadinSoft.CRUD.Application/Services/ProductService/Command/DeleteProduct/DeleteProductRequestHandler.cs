// <copyright file="DeleteProductRequestHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.DeleteProduct;

using MediatR;
using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

/// <summary>
/// Handles requests to delete a <see cref="Product"/> entity.
/// Ensures the requesting user is authorized and the product exists before deletion.
/// </summary>
public class DeleteProductRequestHandler(
    IProductRepository productRepository,
    ICurrentUserService currentUserService,
    ILogger<DeleteProductRequestHandler> logger)
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
                return ApiResponse<object>.Fail("User is unauthorized.");
            }

            Product? product = await productRepository.GetByIdAsync(request.ProductId);

            if (product is null)
            {
                logger.LogWarning("Product not found with Id: {Id}", request.ProductId);
                return ApiResponse<object>.Fail("Product not found.");
            }

            if (product.CreatedByUserId != userId)
            {
                logger.LogWarning(
                    "Unauthorized delete attempt by user {UserId} on product {ProductId} created by {CreatorId}.",
                    userId,
                    product.Id,
                    product.CreatedByUserId);

                return ApiResponse<object>.Fail("You are not owner of this product to delete this product.");
            }

            productRepository.Remove(product);

            return ApiResponse<object>.Success(new object());
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled error occurred while processing product delete request.");

            return ApiResponse<object>.Fail("An unexpected error occurred.");
        }
    }
}