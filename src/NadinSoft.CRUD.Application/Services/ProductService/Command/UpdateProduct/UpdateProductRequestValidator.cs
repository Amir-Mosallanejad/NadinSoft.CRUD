using FluentValidation;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Dto.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Dto.ProduceDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Produce date cannot be in the future.");

        RuleFor(x => x.Dto.ManufacturePhone)
            .MaximumLength(20).WithMessage("phone number cannot exceed 20 characters.")
            .NotEmpty().WithMessage("Manufacture phone is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format. Example: +98901.......");

        RuleFor(x => x.Dto.ManufactureEmail)
            .MaximumLength(100).WithMessage("Manufacture email cannot exceed 100 characters.")
            .NotEmpty().WithMessage("Manufacture email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}