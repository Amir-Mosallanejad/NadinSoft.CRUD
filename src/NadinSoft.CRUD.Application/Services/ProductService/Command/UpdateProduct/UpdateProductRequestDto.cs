namespace NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;

public record UpdateProductRequestDto(
    Guid Id,
    string Name,
    DateTime ProduceDate,
    string ManufacturePhone,
    string ManufactureEmail,
    bool IsAvailable);