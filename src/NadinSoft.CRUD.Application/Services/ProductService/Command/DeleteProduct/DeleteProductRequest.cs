using MediatR;
using NadinSoft.CRUD.Application.Common.DTOs;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.DeleteProduct;

public record DeleteProductRequest(Guid ProductId) : IRequest<ApiResponse<object>>;