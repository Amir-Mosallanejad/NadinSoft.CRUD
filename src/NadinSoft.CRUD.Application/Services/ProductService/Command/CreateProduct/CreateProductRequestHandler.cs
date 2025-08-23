using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;

/// <summary>
/// Handles requests to create new <see cref="Product"/> entities.
/// </summary>
public class CreateProductRequestHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IMapper mapper,
    ILogger<CreateProductRequestHandler> logger,
    ILocalizationService localizationService)
    : IRequestHandler<CreateProductRequest, ApiResponse<object>>
{
    /// <summary>
    /// Handles the <see cref="CreateProductRequest"/> by validating the user,
    /// checking for duplicate products, and adding a new product to the repository.
    /// </summary>
    /// <param name="request">The request containing the product data to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// An <see cref="ApiResponse{T}"/> indicating success or failure of the creation operation.
    /// </returns>
    public async Task<ApiResponse<object>> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync();

            string? userId = currentUserService.UserId;
            if (userId is null)
            {
                return ApiResponse<object>.Fail(
                    localizationService.GetApiMessageResource(ApiMessageResourceKey.UserUnauthorized));
            }

            IBaseRepository<Product> productRepo = unitOfWork.GetRepository<Product>();

            bool isExist = await productRepo.AnyAsync(x =>
                x.ManufactureEmail == request.Dto.ManufactureEmail &&
                x.ProduceDate == request.Dto.ProduceDate);

            if (isExist)
            {
                logger.DuplicateProductLogger(request.Dto.ManufactureEmail, request.Dto.ProduceDate);

                return ApiResponse<object>.Fail(
                    localizationService.GetApiMessageResource(ApiMessageResourceKey.ProductAlreadyExists));
            }

            Product entity = mapper.Map<Product>(request.Dto);
            entity.CreatedByUserId = userId;

            await productRepo.AddAsync(entity);
            await unitOfWork.CommitAsync();

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