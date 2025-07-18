namespace NadinSoft.CRUD.Application.Services.ProductService.DTOs;

public record ProductResponseDto(
    Guid Id,
    string Name,
    DateTime ProduceDate,
    string ManufacturePhone,
    string ManufactureEmail,
    bool IsAvailable,
    string CreatedByUserId);