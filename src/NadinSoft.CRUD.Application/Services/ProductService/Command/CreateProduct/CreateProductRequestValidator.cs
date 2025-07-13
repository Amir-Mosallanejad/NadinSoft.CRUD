using FluentValidation;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Dto.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Dto.ProduceDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Produce date cannot be in the future.");

        RuleFor(x => x.Dto.ManufacturePhone)
            .NotEmpty().WithMessage("Manufacture phone is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

        RuleFor(x => x.Dto.ManufactureEmail)
            .NotEmpty().WithMessage("Manufacture email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.CreatedByUserId)
            .NotEmpty().WithMessage("CreatedByUserId is required.");
    }
}