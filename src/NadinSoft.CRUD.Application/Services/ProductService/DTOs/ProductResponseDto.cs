// <copyright file="ProductResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Application.Services.ProductService.DTOs;

using NadinSoft.CRUD.Domain.Entities;

/// <summary>
/// Represents the data returned for a <see cref="Product"/> in responses.
/// </summary>
/// <param name="Id">The unique identifier of the product.</param>
/// <param name="Name">The name of the product.</param>
/// <param name="ProduceDate">The date the product was manufactured.</param>
/// <param name="ManufacturePhone">The phone number of the manufacturer.</param>
/// <param name="ManufactureEmail">The email address of the manufacturer.</param>
/// <param name="IsAvailable">Indicates whether the product is available for sale or use.</param>
/// <param name="CreatedByUserId">The identifier of the user who created the product.</param>
public record ProductResponseDto(
    Guid Id,
    string Name,
    DateTime ProduceDate,
    string ManufacturePhone,
    string ManufactureEmail,
    bool IsAvailable,
    string CreatedByUserId);