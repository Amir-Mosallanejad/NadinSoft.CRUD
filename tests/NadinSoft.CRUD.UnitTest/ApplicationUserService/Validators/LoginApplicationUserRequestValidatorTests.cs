using FluentValidation.TestHelper;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;

namespace NadinSoft.CRUD.UnitTest.ApplicationUserService.Validators;

public class LoginApplicationUserRequestValidatorTests
{
    private readonly LoginApplicationUserRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        LoginApplicationUserRequest model = new LoginApplicationUserRequest("", "ValidPass123");
        TestValidationResult<LoginApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        LoginApplicationUserRequest model = new LoginApplicationUserRequest("invalid-email", "ValidPass123");
        TestValidationResult<LoginApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("A valid email is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        LoginApplicationUserRequest model = new LoginApplicationUserRequest("user@test.com", "");
        TestValidationResult<LoginApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Too_Short()
    {
        LoginApplicationUserRequest model = new LoginApplicationUserRequest("user@test.com", "123");
        TestValidationResult<LoginApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must be at least 6 characters long.");
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Valid()
    {
        LoginApplicationUserRequest model = new LoginApplicationUserRequest("user@test.com", "ValidPass123");
        TestValidationResult<LoginApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}