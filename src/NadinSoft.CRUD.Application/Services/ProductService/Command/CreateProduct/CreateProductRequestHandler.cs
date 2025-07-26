using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;

public class CreateProductRequestHandler(
    IProductRepository productRepository,
    ICurrentUserService currentUserService,
    IMapper mapper,
    ILogger<CreateProductRequestHandler> logger)
    : IRequestHandler<CreateProductRequest, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            string? userId = currentUserService.UserId;
            if (userId is null)
            {
                return ApiResponse<object>.Fail("User is unauthorized.");
            }

            bool isExist = await productRepository.AnyAsync(x =>
                x.ManufactureEmail == request.Dto.ManufactureEmail &&
                x.ProduceDate == request.Dto.ProduceDate);

            if (isExist)
            {
                logger.LogWarning("Duplicate product detected: ManufactureEmail={Email}, ProduceDate={Date}",
                    request.Dto.ManufactureEmail, request.Dto.ProduceDate);

                return ApiResponse<object>.Fail(
                    "A product with the same Manufacture Email and Produce Date already exists.");
            }

            Product entity = mapper.Map<Product>(request.Dto);
            entity.CreatedByUserId = userId;

            await productRepository.AddAsync(entity);

            return ApiResponse<object>.Success(new object());
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled error occurred while processing product create request.");

            return ApiResponse<object>.Fail("An unexpected error occurred.");
        }
    }
}