using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;

public class UpdateProductRequestHandler(
    IProductRepository productRepository,
    IMapper mapper,
    ILogger<UpdateProductRequestHandler> logger) : IRequestHandler<UpdateProductRequest, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            Product? product = await productRepository.GetByIdAsync(request.Dto.Id);

            if (product is null)
            {
                logger.LogWarning("Product not found with Id: {Id}", request.Dto.Id);
                return new("Product not found.");
            }

            if (product.CreatedByUserId != request.CreatedByUserId)
            {
                logger.LogWarning(
                    "Unauthorized update attempt by user {UserId} on product {ProductId} created by {CreatorId}.",
                    request.CreatedByUserId, product.Id, product.CreatedByUserId);

                return new("You are not owner of this product to update this product.");
            }

            Product model = mapper.Map<Product>(request.Dto);
            model.CreatedByUserId = request.CreatedByUserId;

            product = model;

            productRepository.Update(product);

            return new(new object());
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled error occurred while processing product update request.");

            return new("An unexpected error occurred.");
        }
    }
}