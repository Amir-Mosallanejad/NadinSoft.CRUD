using FluentValidation.TestHelper;
using NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;

namespace NadinSoft.CRUD.UnitTest.ProductService.Validators;

/// <summary>
/// Contains unit tests for <see cref="CreateProductRequestValidator"/>.
/// </summary>
public class CreateProductRequestValidatorTests
{
    /// <summary>
    /// Validator instance being tested.
    /// </summary>
    private readonly CreateProductRequestValidator _validator = new();

    /// <summary>
    /// Tests that validation fails when the product name is empty.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenNameIsEmpty()
    {
        CreateProductRequestDto dto = new CreateProductRequestDto(
            string.Empty,
            DateTime.UtcNow,
            "+989121234567",
            "test@mail.com",
            true);
        CreateProductRequest request = new CreateProductRequest(dto);
        TestValidationResult<CreateProductRequest>? result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    /// <summary>
    /// Tests that validation fails when the product name is too long.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenNameTooLong()
    {
        CreateProductRequestDto dto = new CreateProductRequestDto(
            new string('A', 101),
            DateTime.UtcNow,
            "+989121234567",
            "test@mail.com",
            true);
        CreateProductRequest request = new CreateProductRequest(dto);
        TestValidationResult<CreateProductRequest>? result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    /// <summary>
    /// Tests that validation fails when the produce date is in the future.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenProduceDateIsInTheFuture()
    {
        CreateProductRequestDto dto = new CreateProductRequestDto(
            "Test",
            DateTime.UtcNow.AddDays(1),
            "+989121234567",
            "test@mail.com",
            true);
        CreateProductRequest request = new CreateProductRequest(dto);
        TestValidationResult<CreateProductRequest>? result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ProduceDate);
    }

    /// <summary>
    /// Tests that validation fails when the phone number is invalid.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenPhoneInvalid()
    {
        CreateProductRequestDto dto = new CreateProductRequestDto(
            "Test",
            DateTime.UtcNow,
            "123-invalid",
            "test@mail.com",
            true);
        CreateProductRequest request = new CreateProductRequest(dto);
        TestValidationResult<CreateProductRequest>? result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ManufacturePhone);
    }

    /// <summary>
    /// Tests that validation fails when the email is invalid.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenEmailInvalid()
    {
        CreateProductRequestDto dto = new CreateProductRequestDto(
            "Test",
            DateTime.UtcNow,
            "+989121234567",
            "invalid-email",
            true);
        CreateProductRequest request = new CreateProductRequest(dto);
        TestValidationResult<CreateProductRequest>? result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ManufactureEmail);
    }

    /// <summary>
    /// Tests that no validation errors occur when all fields are valid.
    /// </summary>
    [Fact]
    public void ShouldNotHaveErrorsWhenValid()
    {
        CreateProductRequestDto dto = new CreateProductRequestDto(
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