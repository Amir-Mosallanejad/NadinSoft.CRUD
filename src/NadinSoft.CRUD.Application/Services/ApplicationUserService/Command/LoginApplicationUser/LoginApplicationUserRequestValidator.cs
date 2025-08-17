using FluentValidation;

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;

/// <summary>
/// Validates <see cref="LoginApplicationUserRequest"/> instances.
/// Ensures that the email and password meet required constraints.
/// </summary>
public class LoginApplicationUserRequestValidator : AbstractValidator<LoginApplicationUserRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginApplicationUserRequestValidator"/> class.
    /// Configures rules for validating email and password fields.
    /// </summary>
    public LoginApplicationUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}