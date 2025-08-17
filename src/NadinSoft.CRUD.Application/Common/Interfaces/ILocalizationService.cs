namespace NadinSoft.CRUD.Application.Common.Interfaces;

public interface ILocalizationService
{
    string GetApiMessageResource(string key, params object[] arguments);

    string GetValidatorResource(string key, params object[] arguments);
}