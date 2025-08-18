using FluentValidation.TestHelper;
using Moq;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;
using NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;

namespace NadinSoft.CRUD.UnitTest.ProductService.Validators;

/// <summary>
/// Contains unit tests for <see cref="UpdateProductRequestValidator"/>.
/// </summary>
public class UpdateProductRequestValidatorTests
{
    private readonly Mock<ILocalizationService> _localizationMock = new();

    /// <summary>
    /// Validator instance being tested.
    /// </summary>
    private readonly UpdateProductRequestValidator _validator;

    public UpdateProductRequestValidatorTests()
    {
        _localizationMock.Setup(x => x.GetValidatorResource(ValidatorResourceKey.ProductIdRequired))
            .Returns("ProductId is required.");
        _localizationMock.Setup(x => x.GetValidatorResource(ValidatorResourceKey.NameRequired))
            .Returns("Product name is required.");
        _localizationMock.Setup(x => x.GetValidatorResource(
                ValidatorResourceKey.NameCannotXCharacters,
                It.IsAny<object[]>()))
            .Returns("Product name is too long.");
        _localizationMock.Setup(x => x.GetValidatorResource(ValidatorResourceKey.ProduceCannotFuture))
            .Returns("Produce date cannot be in the future.");
        _localizationMock.Setup(x => x.GetValidatorResource(ValidatorResourceKey.InvalidEmailFormat))
            .Returns("Invalid email address.");
        _localizationMock.Setup(x => x.GetValidatorResource(ValidatorResourceKey.InvalidPhoneNumber))
            .Returns("Invalid phone number.");

        _validator = new UpdateProductRequestValidator(_localizationMock.Object);
    }

    /// <summary>
    /// Tests that validation fails when the Product Id is empty (Guid.Empty).
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenIdIsEmpty()
    {
        UpdateProductRequestDto dto = new UpdateProductRequestDto(
            Guid.Empty,
            "Test",
            DateTime.UtcNow,
            "+989121234567",
            "mail@test.com",
            true);
        UpdateProductRequest model = new UpdateProductRequest(dto);

        TestValidationResult<UpdateProductRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Id);
    }

    /// <summary>
    /// Tests that validation fails when the Product Name is empty.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenNameIsEmpty()
    {
        UpdateProductRequestDto dto = new UpdateProductRequestDto(
            Guid.NewGuid(),
            string.Empty,
            DateTime.UtcNow,
            "+989121234567",
            "mail@test.com",
            true);
        UpdateProductRequest model = new UpdateProductRequest(dto);

        TestValidationResult<UpdateProductRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    /// <summary>
    /// Tests that validation fails when the Product Name exceeds maximum length.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenNameTooLong()
    {
        UpdateProductRequestDto dto = new UpdateProductRequestDto(
            Guid.NewGuid(),
            new string('A', 101),
            DateTime.UtcNow,
            "+989121234567",
            "mail@test.com",
            true);
        UpdateProductRequest model = new UpdateProductRequest(dto);

        TestValidationResult<UpdateProductRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    /// <summary>
    /// Tests that validation fails when ProduceDate is set in the future.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenProduceDateIsInTheFuture()
    {
        UpdateProductRequestDto dto = new UpdateProductRequestDto(
            Guid.NewGuid(),
            "Name",
            DateTime.UtcNow.AddDays(1),
            "+989121234567",
            "mail@test.com",
            true);
        UpdateProductRequest model = new UpdateProductRequest(dto);

        TestValidationResult<UpdateProductRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ProduceDate);
    }

    /// <summary>
    /// Tests that validation fails when ManufactureEmail is invalid.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenEmailIsInvalid()
    {
        UpdateProductRequestDto dto = new UpdateProductRequestDto(
            Guid.NewGuid(),
            "Name",
            DateTime.UtcNow,
            "+989121234567",
            "invalid-email",
            true);
        UpdateProductRequest model = new UpdateProductRequest(dto);

        TestValidationResult<UpdateProductRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ManufactureEmail);
    }

    /// <summary>
    /// Tests that no validation errors occur when all fields are valid.
    /// </summary>
    [Fact]
    public void ShouldNotHaveAnyErrorsWhenValid()
    {
        UpdateProductRequestDto dto = new UpdateProductRequestDto(
            Guid.NewGuid(),
            "Valid Name",
            DateTime.UtcNow.AddSeconds(-1),
            "+989121234567",
            "valid@email.com",
            true);
        UpdateProductRequest model = new UpdateProductRequest(dto);

        TestValidationResult<UpdateProductRequest>? result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails when ManufacturePhone is invalid.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenPhoneInvalid()
    {
        UpdateProductRequestDto dto = new UpdateProductRequestDto(
            Guid.NewGuid(),
            "Name",
            DateTime.UtcNow,
            "invalid",
            "mail@test.com",
            true);
        UpdateProductRequest model = new UpdateProductRequest(dto);

        TestValidationResult<UpdateProductRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ManufacturePhone);
    }
}