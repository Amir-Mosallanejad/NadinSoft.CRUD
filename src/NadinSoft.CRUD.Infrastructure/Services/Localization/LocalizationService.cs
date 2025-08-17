using Microsoft.Extensions.Localization;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Infrastructure.Resources;

namespace NadinSoft.CRUD.Infrastructure.Services.Localization;

public class LocalizationService : ILocalizationService
{
    private readonly IStringLocalizer<ApiMessageResources> _apiMessageLocalizer;
    private readonly IStringLocalizer<ValidatorResources> _validatorLocalizer;

    public LocalizationService(
        IStringLocalizer<ApiMessageResources> apiMessageLocalizer,
        IStringLocalizer<ValidatorResources> validatorLocalizer)
    {
        _apiMessageLocalizer = apiMessageLocalizer;
        _validatorLocalizer = validatorLocalizer;
    }

    public string GetApiMessageResource(string key, params object[] arguments)
    {
        return _apiMessageLocalizer[key, arguments];
    }

    public string GetValidatorResource(string key, params object[] arguments)
    {
        return _validatorLocalizer[key, arguments];
    }
}