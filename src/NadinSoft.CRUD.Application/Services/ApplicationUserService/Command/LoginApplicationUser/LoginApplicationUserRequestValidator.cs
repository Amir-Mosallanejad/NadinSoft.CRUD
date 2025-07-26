using FluentValidation;

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;

public class LoginApplicationUserRequestValidator : AbstractValidator<LoginApplicationUserRequest>
{
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