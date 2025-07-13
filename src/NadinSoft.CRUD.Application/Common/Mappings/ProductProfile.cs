using AutoMapper;
using NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Application.Common.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductRequestDto, Product>();
    }
}