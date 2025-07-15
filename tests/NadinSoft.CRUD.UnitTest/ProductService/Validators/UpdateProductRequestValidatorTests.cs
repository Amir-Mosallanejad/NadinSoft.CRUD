using FluentValidation.TestHelper;
using NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;

namespace NadinSoft.CRUD.UnitTest.ProductService.Validators;

public class UpdateProductRequestValidatorTests
{
    private readonly UpdateProductRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        UpdateProductRequestDto dto = new UpdateProductRequestDto(Guid.Empty, "Test", DateTime.UtcNow, "+989121234567",
            "mail@test.com",
            true);
        UpdateProductRequest model = new UpdateProductRequest(dto);

        TestValidationResult<UpdateProductRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        UpdateProductRequestDto dto = new UpdateProductRequestDto(Guid.NewGuid(), "", DateTime.UtcNow, "+989121234567",
            "mail@test.com",
            true);
        UpdateProductRequest model = new UpdateProductRequest(dto);

        TestValidationResult<UpdateProductRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Too_Long()
    {
        UpdateProductRequestDto dto = new UpdateProductRequestDto(Guid.NewGuid(), new string('A', 101), DateTime.UtcNow,
            "+989121234567",
            "mail@test.com", true);
        UpdateProductRequest model = new UpdateProductRequest(dto);

        TestValidationResult<UpdateProductRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void Should_Have_Error_When_ProduceDate_Is_In_The_Future()
    {
        UpdateProductRequestDto dto = new UpdateProductRequestDto(Guid.NewGuid(), "Name", DateTime.UtcNow.AddDays(1),
            "+989121234567",
            "mail@test.com", true);
        UpdateProductRequest model = new UpdateProductRequest(dto);

        TestValidationResult<UpdateProductRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ProduceDate);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        UpdateProductRequestDto dto = new UpdateProductRequestDto(Guid.NewGuid(), "Name", DateTime.UtcNow,
            "+989121234567", "invalid-email",
            true);
        UpdateProductRequest model = new UpdateProductRequest(dto);

        TestValidationResult<UpdateProductRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ManufactureEmail);
    }

    [Fact]
    public void Should_Not_Have_Any_Errors_When_Valid()
    {
        UpdateProductRequestDto dto = new UpdateProductRequestDto(Guid.NewGuid(), "Valid Name",
            DateTime.UtcNow.AddSeconds(-1),
            "+989121234567",
            "valid@email.com", true);
        UpdateProductRequest model = new UpdateProductRequest(dto);

        TestValidationResult<UpdateProductRequest>? result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_Phone_Invalid()
    {
        UpdateProductRequestDto dto = new UpdateProductRequestDto(Guid.NewGuid(), "Name", DateTime.UtcNow, "invalid",
            "mail@test.com",
            true);
        UpdateProductRequest model = new UpdateProductRequest(dto);

        TestValidationResult<UpdateProductRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ManufacturePhone);
    }
}