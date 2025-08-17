using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;
/// <summary>
/// Represents the data required to create a new <see cref="Product"/>.
/// </summary>
/// <param name="Name">The name of the product.</param>
/// <param name="ProduceDate">The date the product was manufactured.</param>
/// <param name="ManufacturePhone">The phone number of the manufacturer.</param>
/// <param name="ManufactureEmail">The email address of the manufacturer.</param>
/// <param name="IsAvailable">
/// A value indicating whether the product is available for sale or use.
/// </param>
public record CreateProductRequestDto(
    string Name,
    DateTime ProduceDate,
    string ManufacturePhone,
    string ManufactureEmail,
    bool IsAvailable);