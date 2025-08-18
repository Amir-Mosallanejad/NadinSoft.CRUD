using Microsoft.Extensions.Localization;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Infrastructure.Resources;

namespace NadinSoft.CRUD.Infrastructure.Services.Localization;

/// <summary>
/// Provides localization services for API messages and validation messages.
/// </summary>
public class LocalizationService : ILocalizationService
{
    /// <summary>
    /// Localizer instance for API-related message resources.
    /// </summary>
    private readonly IStringLocalizer<ApiMessageResources> _apiMessageLocalizer;

    /// <summary>
    /// Localizer instance for validation-related message resources.
    /// </summary>
    private readonly IStringLocalizer<ValidatorResources> _validatorLocalizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizationService"/> class.
    /// </summary>
    /// <param name="apiMessageLocalizer">The localizer for API message resources.</param>
    /// <param name="validatorLocalizer">The localizer for validator message resources.</param>
    public LocalizationService(
        IStringLocalizer<ApiMessageResources> apiMessageLocalizer,
        IStringLocalizer<ValidatorResources> validatorLocalizer)
    {
        _apiMessageLocalizer = apiMessageLocalizer;
        _validatorLocalizer = validatorLocalizer;
    }

    /// <summary>
    /// Retrieves a localized string from the API message resources.
    /// </summary>
    /// <param name="key">The resource key to look up.</param>
    /// <param name="arguments">Optional formatting arguments for the localized string.</param>
    /// <returns>The localized string for the given key.</returns>
    public string GetApiMessageResource(string key, params object[] arguments)
    {
        return _apiMessageLocalizer[key, arguments];
    }

    /// <summary>
    /// Retrieves a localized string from the validator message resources.
    /// </summary>
    /// <param name="key">The resource key to look up.</param>
    /// <param name="arguments">Optional formatting arguments for the localized string.</param>
    /// <returns>The localized string for the given key.</returns>
    public string GetValidatorResource(string key, params object[] arguments)
    {
        return _validatorLocalizer[key, arguments];
    }
}