// <copyright file="RegisterApplicationUserRequestValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;

using FluentValidation;

/// <summary>
/// Validates <see cref="RegisterApplicationUserRequest"/> instances.
/// Ensures that the email, password, and confirm password fields meet required constraints.
/// </summary>
public class RegisterApplicationUserRequestValidator : AbstractValidator<RegisterApplicationUserRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterApplicationUserRequestValidator"/> class.
    /// Configures rules for validating email, password, and confirm password fields.
    /// </summary>
    public RegisterApplicationUserRequestValidator()
    {
        // Validate the Email property
        this.RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        // Validate the Password property
        this.RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        // Validate the ConfirmPassword property
        this.RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm Password is required.")
            .Equal(x => x.Password).WithMessage("Passwords do not match.");
    }
}