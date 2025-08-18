using FluentValidation;
using NadinSoft.CRUD.Application.Common.Interfaces;

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;

/// <summary>
/// Validates <see cref="RegisterApplicationUserRequest"/> instances.
/// Ensures that the email, password, and confirm password fields meet required constraints.
/// </summary>
public class RegisterApplicationUserRequestValidator : AbstractValidator<RegisterApplicationUserRequest>
{
    /// <summary>
    /// Provides access to localized validation messages.
    /// </summary>
    private readonly ILocalizationService _localizationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterApplicationUserRequestValidator"/> class.
    /// Configures rules for validating email, password, and confirm password fields.
    /// </summary>
    /// <param name="localizationService">
    /// Service used to retrieve localized validation messages from resource files.
    /// </param>
    public RegisterApplicationUserRequestValidator(ILocalizationService localizationService)
    {
        _localizationService = localizationService;

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(GetSafeMessage("EmailRequired"))
            .EmailAddress()
            .WithMessage(GetSafeMessage("ValidEmailRequired"));

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(GetSafeMessage("PasswordRequired"))
            .MinimumLength(6)
            .WithMessage(GetSafeMessage("PasswordBeXCharacters", 6));

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage(GetSafeMessage("ConfirmPasswordRequired"))
            .Equal(x => x.Password)
            .WithMessage(GetSafeMessage("PasswordsNotMatch"));
    }

    /// <summary>
    /// Safely gets a localized message, falling back to the key if the value is empty.
    /// </summary>
    private string GetSafeMessage(string key, params object[] args)
    {
        string msg = _localizationService.GetValidatorResource(key, args);
        return string.IsNullOrWhiteSpace(msg) ? key : msg;
    }
}