using FluentValidation.TestHelper;
using Moq;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;

namespace NadinSoft.CRUD.UnitTest.ApplicationUserService.Validators;

/// <summary>
/// Contains unit tests for <see cref="RegisterApplicationUserRequestValidator"/>.
/// </summary>
public class RegisterApplicationUserRequestValidatorTests
{
    private readonly Mock<ILocalizationService> _localizationMock = new();

    /// <summary>
    /// Validator instance being tested.
    /// </summary>
    private readonly RegisterApplicationUserRequestValidator _validator;

    public RegisterApplicationUserRequestValidatorTests()
    {
        _localizationMock.Setup(x => x.GetValidatorResource(ValidatorResourceKey.EmailRequired, It.IsAny<object[]>()))
            .Returns("Email is required.");
        _localizationMock.Setup(x => x.GetValidatorResource(
                ValidatorResourceKey.ValidEmailRequired,
                It.IsAny<object[]>()))
            .Returns("A valid email is required.");
        _localizationMock
            .Setup(x => x.GetValidatorResource(ValidatorResourceKey.PasswordRequired, It.IsAny<object[]>()))
            .Returns("Password is required.");
        _localizationMock.Setup(x => x.GetValidatorResource(
                ValidatorResourceKey.PasswordBeXCharacters,
                It.IsAny<object[]>()))
            .Returns("Password must be at least 6 characters long.");
        _localizationMock.Setup(x => x.GetValidatorResource(
                ValidatorResourceKey.ConfirmPasswordRequired,
                It.IsAny<object[]>()))
            .Returns("Confirm Password is required.");
        _localizationMock.Setup(x => x.GetValidatorResource(
                ValidatorResourceKey.PasswordsNotMatch,
                It.IsAny<object[]>()))
            .Returns("Passwords do not match.");

        _validator = new RegisterApplicationUserRequestValidator(_localizationMock.Object);
    }

    /// <summary>
    /// Tests that validation fails when the email is empty.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenEmailIsEmpty()
    {
        RegisterApplicationUserRequest model = new RegisterApplicationUserRequest(
            string.Empty,
            "Password123",
            "Password123");
        TestValidationResult<RegisterApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required.");
    }

    /// <summary>
    /// Tests that validation fails when the email format is invalid.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenEmailIsInvalid()
    {
        RegisterApplicationUserRequest model = new RegisterApplicationUserRequest(
            "invalid-email",
            "Password123",
            "Password123");
        TestValidationResult<RegisterApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("A valid email is required.");
    }

    /// <summary>
    /// Tests that validation fails when the password is empty.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenPasswordIsEmpty()
    {
        RegisterApplicationUserRequest model = new RegisterApplicationUserRequest(
            "user@test.com",
            string.Empty,
            string.Empty);
        TestValidationResult<RegisterApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required.");
    }

    /// <summary>
    /// Tests that validation fails when the password is too short.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenPasswordIsTooShort()
    {
        RegisterApplicationUserRequest model = new RegisterApplicationUserRequest("user@test.com", "123", "123");
        TestValidationResult<RegisterApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must be at least 6 characters long.");
    }

    /// <summary>
    /// Tests that validation fails when the confirm password is empty.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenConfirmPasswordIsEmpty()
    {
        RegisterApplicationUserRequest model = new RegisterApplicationUserRequest(
            "user@test.com",
            "Password123",
            string.Empty);
        TestValidationResult<RegisterApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword)
            .WithErrorMessage("Confirm Password is required.");
    }

    /// <summary>
    /// Tests that validation fails when password and confirm password do not match.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenPasswordsDoNotMatch()
    {
        RegisterApplicationUserRequest model = new RegisterApplicationUserRequest(
            "user@test.com",
            "Password123",
            "Mismatch123");
        TestValidationResult<RegisterApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword)
            .WithErrorMessage("Passwords do not match.");
    }

    /// <summary>
    /// Tests that validation passes when all fields are valid.
    /// </summary>
    [Fact]
    public void ShouldNotHaveErrorsWhenAllFieldsAreValid()
    {
        RegisterApplicationUserRequest model = new RegisterApplicationUserRequest(
            "user@test.com",
            "Password123",
            "Password123");
        TestValidationResult<RegisterApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}