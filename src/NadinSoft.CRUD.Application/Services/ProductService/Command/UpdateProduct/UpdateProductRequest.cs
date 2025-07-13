using MediatR;
using NadinSoft.CRUD.Application.Common.DTOs;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;

public record UpdateProductRequest(UpdateProductRequestDto Dto, string CreatedByUserId) : IRequest<ApiResponse<object>>;