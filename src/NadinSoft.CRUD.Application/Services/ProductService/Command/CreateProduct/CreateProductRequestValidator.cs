using FluentValidation;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Application.Common.ResourceKeys;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;

/// <summary>
/// Validates <see cref="CreateProductRequest"/> instances to ensure product creation data is correct.
/// </summary>
public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    /// <summary>
    /// Provides access to localized validation messages.
    /// </summary>
    private readonly ILocalizationService _localizationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductRequestValidator"/> class.
    /// Configures validation rules for product creation, including name, produce date, manufacturer phone, and email.
    /// </summary>
    /// <param name="localizationService">
    /// Service used to retrieve localized validation messages from resource files.
    /// </param>
    public CreateProductRequestValidator(ILocalizationService localizationService)
    {
        _localizationService = localizationService;

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .WithMessage(GetSafeMessage(ValidatorResourceKey.NameRequired))
            .MaximumLength(50)
            .WithMessage(GetSafeMessage(ValidatorResourceKey.NameCannotXCharacters, 50));

        RuleFor(x => x.Dto.ProduceDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(GetSafeMessage(ValidatorResourceKey.ProduceCannotFuture));

        RuleFor(x => x.Dto.ManufacturePhone)
            .MaximumLength(20)
            .WithMessage(GetSafeMessage(ValidatorResourceKey.PhoneNumberXCharacters, 20))
            .NotEmpty()
            .WithMessage(GetSafeMessage(ValidatorResourceKey.ManufacturePhoneRequired))
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage(GetSafeMessage(ValidatorResourceKey.InvalidPhoneNumber));

        RuleFor(x => x.Dto.ManufactureEmail)
            .MaximumLength(100)
            .WithMessage(GetSafeMessage(ValidatorResourceKey.ManufactureEmailXCharacters, 100))
            .NotEmpty()
            .WithMessage(GetSafeMessage(ValidatorResourceKey.ManufactureEmailRequired))
            .EmailAddress()
            .WithMessage(GetSafeMessage(ValidatorResourceKey.InvalidEmailFormat));
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