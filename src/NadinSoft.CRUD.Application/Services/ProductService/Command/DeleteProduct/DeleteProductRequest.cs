// <copyright file="DeleteProductRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.DeleteProduct;

using MediatR;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Domain.Entities;

/// <summary>
/// Represents a request to delete an existing <see cref="Product"/> by its unique identifier.
/// </summary>
/// <param name="ProductId">The unique identifier of the product to be deleted.</param>
/// <remarks>
/// This request implements <see cref="IRequest{TResponse}"/> and expects an <see cref="ApiResponse{T}"/>
/// indicating whether the deletion was successful.
/// </remarks>
public record DeleteProductRequest(Guid ProductId) : IRequest<ApiResponse<object>>;