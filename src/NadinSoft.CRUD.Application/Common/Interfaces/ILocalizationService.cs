namespace NadinSoft.CRUD.Application.Common.Interfaces;

/// <summary>
/// Provides methods to retrieve localized resources for API messages and validation messages.
/// </summary>
public interface ILocalizationService
{
    /// <summary>
    /// Retrieves a localized string from the API message resources.
    /// </summary>
    /// <param name="key">The resource key to look up.</param>
    /// <param name="arguments">Optional formatting arguments for the localized string.</param>
    /// <returns>
    /// The localized API message corresponding to the specified <paramref name="key"/>.
    /// </returns>
    string GetApiMessageResource(string key, params object[] arguments);

    /// <summary>
    /// Retrieves a localized string from the validator message resources.
    /// </summary>
    /// <param name="key">The resource key to look up.</param>
    /// <param name="arguments">Optional formatting arguments for the localized string.</param>
    /// <returns>
    /// The localized validator message corresponding to the specified <paramref name="key"/>.
    /// </returns>
    string GetValidatorResource(string key, params object[] arguments);
}