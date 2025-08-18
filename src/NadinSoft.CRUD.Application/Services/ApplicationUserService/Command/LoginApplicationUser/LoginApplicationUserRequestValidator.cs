using FluentValidation;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;

/// <summary>
/// Validates <see cref="LoginApplicationUserRequest"/> instances.
/// Ensures that the email and password meet required constraints.
/// </summary>
public class LoginApplicationUserRequestValidator : AbstractValidator<LoginApplicationUserRequest>
{
    /// <summary>
    /// Provides access to localized validation messages.
    /// </summary>
    private readonly ILocalizationService _localizationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginApplicationUserRequestValidator"/> class.
    /// Configures rules for validating email and password fields.
    /// </summary>
    /// <param name="localizationService">
    /// Service used to retrieve localized validation messages from resource files.
    /// </param>
    public LoginApplicationUserRequestValidator(ILocalizationService localizationService)
    {
        _localizationService = localizationService;

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(GetSafeMessage(ValidatorResourceKey.EmailRequired))
            .EmailAddress()
            .WithMessage(GetSafeMessage(ValidatorResourceKey.ValidEmailRequired));

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(GetSafeMessage(ValidatorResourceKey.PasswordRequired))
            .MinimumLength(6)
            .WithMessage(GetSafeMessage(ValidatorResourceKey.PasswordBeXCharacters, 6));
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