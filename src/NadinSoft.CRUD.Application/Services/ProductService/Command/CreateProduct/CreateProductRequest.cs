// <copyright file="CreateProductRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;

using MediatR;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Domain.Entities;

/// <summary>
/// Represents a request to create a new <see cref="Product"/>.
/// </summary>
/// <param name="Dto">The data transfer object containing product details for creation.</param>
/// <remarks>
/// This request implements <see cref="IRequest{TResponse}"/> and expects an <see cref="ApiResponse{T}"/>
/// indicating the result of the creation operation.
/// </remarks>
public record CreateProductRequest(CreateProductRequestDto Dto)
    : IRequest<ApiResponse<object>>;