using FluentValidation.TestHelper;
using Moq;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;
using NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;

namespace NadinSoft.CRUD.UnitTest.ProductService.Validators;

/// <summary>
/// Contains unit tests for <see cref="CreateProductRequestValidator"/>.
/// </summary>
public class CreateProductRequestValidatorTests
{
    /// <summary>
    /// Mock instance of <see cref="ILocalizationService"/> used for unit testing.
    /// </summary>
    private readonly Mock<ILocalizationService> _localizationMock = new();

    /// <summary>
    /// Validator instance being tested.
    /// </summary>
    private readonly CreateProductRequestValidator _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductRequestValidatorTests"/> class.
    /// Sets up the mocked localization service to return validator messages for product creation.
    /// </summary>
    public CreateProductRequestValidatorTests()
    {
        _localizationMock.Setup(x => x.GetValidatorResource(ValidatorResourceKey.NameRequired))
            .Returns("Product name is required.");
        _localizationMock.Setup(x => x.GetValidatorResource(
                ValidatorResourceKey.NameCannotXCharacters,
                It.IsAny<object[]>()))
            .Returns("Product name is too long.");
        _localizationMock.Setup(x => x.GetValidatorResource(ValidatorResourceKey.ProduceCannotFuture))
            .Returns("Produce date cannot be in the future.");
        _localizationMock.Setup(x => x.GetValidatorResource(ValidatorResourceKey.InvalidPhoneNumber))
            .Returns("Invalid phone number.");
        _localizationMock.Setup(x => x.GetValidatorResource(ValidatorResourceKey.InvalidEmailFormat))
            .Returns("Invalid email address.");

        _validator = new CreateProductRequestValidator(_localizationMock.Object);
    }

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
            true,
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
            true,
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
            true,
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
            true,
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
            true,
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
            true,
            true);
        CreateProductRequest request = new CreateProductRequest(dto);
        TestValidationResult<CreateProductRequest>? result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}