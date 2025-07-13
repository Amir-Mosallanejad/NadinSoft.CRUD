using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.DeleteProduct;

public class DeleteProductRequestHandler(
    IProductRepository productRepository,
    IMapper mapper,
    ILogger<DeleteProductRequestHandler> logger)
    : IRequestHandler<DeleteProductRequest, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            Product? product = await productRepository.GetByIdAsync(request.ProductId);

            if (product is null)
            {
                logger.LogWarning("Product not found with Id: {Id}", request.ProductId);
                return new("Product not found.");
            }

            if (product.CreatedByUserId != request.CreatedByUserId)
            {
                logger.LogWarning(
                    "Unauthorized delete attempt by user {UserId} on product {ProductId} created by {CreatorId}.",
                    request.CreatedByUserId, product.Id, product.CreatedByUserId);

                return new("You are not owner of this product to delete this product.");
            }

            productRepository.Remove(product);

            return new(new object());
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled error occurred while processing product delete request.");

            return new("An unexpected error occurred.");
        }
    }
}