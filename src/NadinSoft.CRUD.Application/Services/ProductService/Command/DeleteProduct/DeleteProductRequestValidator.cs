using FluentValidation;
using NadinSoft.CRUD.Application.Common.Interfaces;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.DeleteProduct;

/// <summary>
/// Validates <see cref="DeleteProductRequest"/> instances to ensure the request contains a valid product ID.
/// </summary>
public class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
{
    /// <summary>
    /// Provides access to localized validation messages.
    /// </summary>
    private readonly ILocalizationService _localizationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProductRequestValidator"/> class.
    /// Configures the rule to ensure <see cref="DeleteProductRequest.ProductId"/> is not empty.
    /// </summary>
    /// <param name="localizationService">
    /// Service used to retrieve localized validation messages from resource files.
    /// </param>
    public DeleteProductRequestValidator(ILocalizationService localizationService)
    {
        _localizationService = localizationService;

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(GetSafeMessage("ProductIdRequired"));
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