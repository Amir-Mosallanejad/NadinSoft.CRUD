using FluentValidation.TestHelper;
using NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;

namespace NadinSoft.CRUD.UnitTest.ProductService.Validators;

public class CreateProductRequestValidatorTests
{
    private readonly CreateProductRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        CreateProductRequestDto dto = new CreateProductRequestDto(
            Guid.Empty,
            "Test",
            DateTime.UtcNow,
            "+989121234567",
            "test@mail.com",
            true);
        CreateProductRequest request = new CreateProductRequest(dto);
        TestValidationResult<CreateProductRequest>? result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        CreateProductRequestDto dto = new CreateProductRequestDto(
            Guid.NewGuid(),
            "",
            DateTime.UtcNow,
            "+989121234567",
            "test@mail.com",
            true);
        CreateProductRequest request = new CreateProductRequest(dto);
        TestValidationResult<CreateProductRequest>? result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Too_Long()
    {
        CreateProductRequestDto dto = new CreateProductRequestDto(
            Guid.NewGuid(),
            new string('A', 101),
            DateTime.UtcNow,
            "+989121234567",
            "test@mail.com",
            true);
        CreateProductRequest request = new CreateProductRequest(dto);
        TestValidationResult<CreateProductRequest>? result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void Should_Have_Error_When_ProduceDate_Is_In_The_Future()
    {
        CreateProductRequestDto dto = new CreateProductRequestDto(
            Guid.NewGuid(),
            "Test",
            DateTime.UtcNow.AddDays(1),
            "+989121234567",
            "test@mail.com",
            true);
        CreateProductRequest request = new CreateProductRequest(dto);
        TestValidationResult<CreateProductRequest>? result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ProduceDate);
    }

    [Fact]
    public void Should_Have_Error_When_Phone_Invalid()
    {
        CreateProductRequestDto dto = new CreateProductRequestDto(
            Guid.NewGuid(),
            "Test",
            DateTime.UtcNow,
            "123-invalid",
            "test@mail.com",
            true);
        CreateProductRequest request = new CreateProductRequest(dto);
        TestValidationResult<CreateProductRequest>? result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ManufacturePhone);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Invalid()
    {
        CreateProductRequestDto dto = new CreateProductRequestDto(
            Guid.NewGuid(),
            "Test",
            DateTime.UtcNow,
            "+989121234567",
            "invalid-email",
            true);
        CreateProductRequest request = new CreateProductRequest(dto);
        TestValidationResult<CreateProductRequest>? result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ManufactureEmail);
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Valid()
    {
        CreateProductRequestDto dto = new CreateProductRequestDto(
            Guid.NewGuid(),
            "Valid",
            DateTime.UtcNow.AddSeconds(-1),
            "+989121234567",
            "valid@mail.com",
            true);
        CreateProductRequest request = new CreateProductRequest(dto);
        TestValidationResult<CreateProductRequest>? result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}