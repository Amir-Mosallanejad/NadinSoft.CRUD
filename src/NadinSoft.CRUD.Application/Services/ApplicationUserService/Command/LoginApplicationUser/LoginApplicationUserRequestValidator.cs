// <copyright file="LoginApplicationUserRequestValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;

using FluentValidation;

/// <summary>
/// Validates <see cref="LoginApplicationUserRequest"/> instances.
/// Ensures that the email and password meet required constraints.
/// </summary>
public class LoginApplicationUserRequestValidator : AbstractValidator<LoginApplicationUserRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginApplicationUserRequestValidator"/> class.
    /// Configures rules for validating email and password fields.
    /// </summary>
    public LoginApplicationUserRequestValidator()
    {
        // Validate the Email property
        this.RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        // Validate the Password property
        this.RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}