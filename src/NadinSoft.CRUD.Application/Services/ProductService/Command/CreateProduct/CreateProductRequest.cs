using MediatR;
using NadinSoft.CRUD.Application.Common.DTOs;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;

public record CreateProductRequest(
    Guid Id,
    string Name,
    DateTime ProduceDate,
    string ManufacturePhone,
    string ManufactureEmail,
    bool IsAvailable,
    string CreatedByUserId)
    : IRequest<ApiResponse<object>>;