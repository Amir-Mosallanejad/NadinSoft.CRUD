using AutoMapper;
using NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;
using NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;
using NadinSoft.CRUD.Application.Services.ProductService.DTOs;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Application.Common.Mappings;

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
        CreateMap<CreateProductRequestDto, Product>();

        CreateMap<UpdateProductRequestDto, Product>();

        CreateMap<Product, ProductResponseDto>();
    }
}