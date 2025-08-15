// <copyright file="CreateProductRequestValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;

using FluentValidation;

/// <summary>
/// Validates <see cref="CreateProductRequest"/> instances to ensure product creation data is correct.
/// </summary>
public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductRequestValidator"/> class.
    /// Configures validation rules for product creation, including name, produce date, manufacturer phone, and email.
    /// </summary>
    public CreateProductRequestValidator()
    {
        // Validate the product name
        this.RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

        // Validate the produce date
        this.RuleFor(x => x.Dto.ProduceDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Produce date cannot be in the future.");

        // Validate the manufacturer's phone number
        this.RuleFor(x => x.Dto.ManufacturePhone)
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.")
            .NotEmpty().WithMessage("Manufacture phone is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format. Example: +98901.......");

        // Validate the manufacturer's email
        this.RuleFor(x => x.Dto.ManufactureEmail)
            .MaximumLength(100).WithMessage("Manufacture email cannot exceed 100 characters.")
            .NotEmpty().WithMessage("Manufacture email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}