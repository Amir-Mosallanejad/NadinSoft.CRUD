namespace NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;

public record CreateProductRequestDto(
    string Name,
    DateTime ProduceDate,
    string ManufacturePhone,
    string ManufactureEmail,
    bool IsAvailable
);