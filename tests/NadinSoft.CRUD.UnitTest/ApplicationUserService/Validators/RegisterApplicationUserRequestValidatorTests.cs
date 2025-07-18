using FluentValidation.TestHelper;
using NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;

namespace NadinSoft.CRUD.UnitTest.ApplicationUserService.Validators;

public class RegisterApplicationUserRequestValidatorTests
{
    private readonly RegisterApplicationUserRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        RegisterApplicationUserRequest model = new RegisterApplicationUserRequest("", "Password123", "Password123");
        TestValidationResult<RegisterApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Email is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        RegisterApplicationUserRequest model = new RegisterApplicationUserRequest("invalid-email", "Password123", "Password123");
        TestValidationResult<RegisterApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("A valid email is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        RegisterApplicationUserRequest model = new RegisterApplicationUserRequest("user@test.com", "", "");
        TestValidationResult<RegisterApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Password is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        RegisterApplicationUserRequest model = new RegisterApplicationUserRequest("user@test.com", "123", "123");
        TestValidationResult<RegisterApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Password must be at least 6 characters long.");
    }

    [Fact]
    public void Should_Have_Error_When_ConfirmPassword_Is_Empty()
    {
        RegisterApplicationUserRequest model = new RegisterApplicationUserRequest("user@test.com", "Password123", "");
        TestValidationResult<RegisterApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword)
              .WithErrorMessage("Confirm Password is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Passwords_Do_Not_Match()
    {
        RegisterApplicationUserRequest model = new RegisterApplicationUserRequest("user@test.com", "Password123", "Mismatch123");
        TestValidationResult<RegisterApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword)
              .WithErrorMessage("Passwords do not match.");
    }

    [Fact]
    public void Should_Not_Have_Errors_When_All_Fields_Are_Valid()
    {
        RegisterApplicationUserRequest model = new RegisterApplicationUserRequest("user@test.com", "Password123", "Password123");
        TestValidationResult<RegisterApplicationUserRequest>? result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}