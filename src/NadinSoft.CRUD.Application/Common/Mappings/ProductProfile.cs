// <copyright file="ProductProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Application.Common.Mappings;

using AutoMapper;
using NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;
using NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;
using NadinSoft.CRUD.Application.Services.ProductService.DTOs;
using NadinSoft.CRUD.Domain.Entities;

/// <summary>
/// Defines AutoMapper mapping configuration for <see cref="Product"/> entities.
/// </summary>
public class ProductProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductProfile"/> class.
    /// Configures mappings between product-related DTOs and the <see cref="Product"/> entity.
    /// </summary>
    public ProductProfile()
    {
        // Maps a DTO used to create a product to the Product entity.
        this.CreateMap<CreateProductRequestDto, Product>();

        // Maps a DTO used to update a product to the Product entity.
        this.CreateMap<UpdateProductRequestDto, Product>();

        // Maps the Product entity to a DTO used in responses.
        this.CreateMap<Product, ProductResponseDto>();
    }
}