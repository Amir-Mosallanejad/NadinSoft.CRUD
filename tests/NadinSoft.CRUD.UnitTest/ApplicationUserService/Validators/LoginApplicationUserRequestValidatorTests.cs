using FluentValidation.TestHelper;
using Moq;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;

namespace NadinSoft.CRUD.UnitTest.ApplicationUserService.Validators;

/// <summary>
/// Contains unit tests for <see cref="LoginApplicationUserRequestValidator"/>.
/// </summary>
public class LoginApplicationUserRequestValidatorTests
{
    private readonly Mock<ILocalizationService> _localizationServiceMock = new();

    /// <summary>
    /// Validator instance being tested.
    /// </summary>
    private readonly LoginApplicationUserRequestValidator _validator;

    public LoginApplicationUserRequestValidatorTests()
    {
        _localizationServiceMock.Setup(x => x.GetValidatorResource(ValidatorResourceKey.EmailRequired))
            .Returns("Email is required.");
        _localizationServiceMock.Setup(x => x.GetValidatorResource(ValidatorResourceKey.PasswordRequired))
            .Returns("Password is required.");
        _localizationServiceMock.Setup(x => x.GetValidatorResource(ValidatorResourceKey.ValidEmailRequired))
            .Returns("A valid email is required.");
        _localizationServiceMock.Setup(x => x.GetValidatorResource(ValidatorResourceKey.PasswordBeXCharacters, 6))
            .Returns("Password must be at least 6 characters long.");

        _validator = new LoginApplicationUserRequestValidator(_localizationServiceMock.Object);
    }

    /// <summary>
    /// Tests that validation fails when the email is empty.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenEmailIsEmpty()
    {
        LoginApplicationUserRequest model = new LoginApplicationUserRequest(string.Empty, "ValidPass123");

        TestValidationResult<LoginApplicationUserRequest>? result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required.");
    }

    /// <summary>
    /// Tests that validation fails when the email format is invalid.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenEmailIsInvalid()
    {
        LoginApplicationUserRequest model = new LoginApplicationUserRequest("invalid-email", "ValidPass123");
        TestValidationResult<LoginApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("A valid email is required.");
    }

    /// <summary>
    /// Tests that validation fails when the password is empty.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenPasswordIsEmpty()
    {
        LoginApplicationUserRequest model = new LoginApplicationUserRequest("user@test.com", string.Empty);
        TestValidationResult<LoginApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required.");
    }

    /// <summary>
    /// Tests that validation fails when the password is too short.
    /// </summary>
    [Fact]
    public void ShouldHaveErrorWhenPasswordTooShort()
    {
        LoginApplicationUserRequest model = new LoginApplicationUserRequest("user@test.com", "123");
        TestValidationResult<LoginApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must be at least 6 characters long.");
    }

    /// <summary>
    /// Tests that validation passes when both email and password are valid.
    /// </summary>
    [Fact]
    public void ShouldNotHaveErrorsWhenValid()
    {
        LoginApplicationUserRequest model = new LoginApplicationUserRequest("user@test.com", "ValidPass123");
        TestValidationResult<LoginApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}