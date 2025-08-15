// <copyright file="UpdateProductRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;

using MediatR;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Domain.Entities;

/// <summary>
/// Represents a request to update an existing <see cref="Product"/>.
/// </summary>
/// <param name="Dto">The data transfer object containing updated product details.</param>
/// <remarks>
/// This request implements <see cref="IRequest{TResponse}"/> and expects an <see cref="ApiResponse{T}"/>
/// indicating the result of the update operation.
/// </remarks>
public record UpdateProductRequest(UpdateProductRequestDto Dto) : IRequest<ApiResponse<object>>;