using MediatR;
using NadinSoft.CRUD.Application.Common.DTOs;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;

public record CreateProductRequest(
    CreateProductRequestDto Dto,
    string CreatedByUserId)
    : IRequest<ApiResponse<object>>;