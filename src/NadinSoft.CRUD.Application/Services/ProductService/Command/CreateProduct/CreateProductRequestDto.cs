namespace NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;

public record CreateProductRequestDto(
    Guid Id,
    string Name,
    DateTime ProduceDate,
    string ManufacturePhone,
    string ManufactureEmail,
    bool IsAvailable
);