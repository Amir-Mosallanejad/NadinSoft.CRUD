using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;

/// <summary>
/// Represents the data required to update an existing <see cref="Product"/>.
/// </summary>
/// <param name="Id">The unique identifier of the product to update.</param>
/// <param name="Name">The updated name of the product.</param>
/// <param name="ProduceDate">The updated produce date of the product.</param>
/// <param name="ManufacturePhone">The updated phone number of the manufacturer.</param>
/// <param name="ManufactureEmail">The updated email address of the manufacturer.</param>
/// <param name="IsAvailable">
/// A value indicating whether the product is currently available for sale or use.
/// </param>
public record UpdateProductRequestDto(
    Guid Id,
    string Name,
    DateTime ProduceDate,
    string ManufacturePhone,
    string ManufactureEmail,
    bool IsAvailable,
    bool IsValid);