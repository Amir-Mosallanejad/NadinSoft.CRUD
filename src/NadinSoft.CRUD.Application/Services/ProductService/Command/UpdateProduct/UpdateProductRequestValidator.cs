using FluentValidation;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;

/// <summary>
/// Validates <see cref="UpdateProductRequest"/> instances to ensure product update data is correct.
/// </summary>
public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProductRequestValidator"/> class.
    /// Configures validation rules for product update, including ID, name, produce date, manufacturer phone, and email.
    /// </summary>
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Dto.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

        RuleFor(x => x.Dto.ProduceDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Produce date cannot be in the future.");

        RuleFor(x => x.Dto.ManufacturePhone)
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.")
            .NotEmpty().WithMessage("Manufacture phone is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format. Example: +98901.......");

        RuleFor(x => x.Dto.ManufactureEmail)
            .MaximumLength(100).WithMessage("Manufacture email cannot exceed 100 characters.")
            .NotEmpty().WithMessage("Manufacture email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}