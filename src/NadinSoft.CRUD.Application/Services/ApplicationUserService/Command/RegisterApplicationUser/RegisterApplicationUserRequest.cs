// <copyright file="RegisterApplicationUserRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;

using MediatR;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Domain.Entities;

/// <summary>
/// Represents a request to register a new <see cref="ApplicationUser"/>.
/// </summary>
/// <param name="Email">The email address of the user to register.</param>
/// <param name="Password">The password for the new user.</param>
/// <param name="ConfirmPassword">The confirmation of the password, must match <paramref name="Password"/>.</param>
/// <remarks>
/// This request implements <see cref="IRequest{TResponse}"/> and expects an <see cref="ApiResponse{T}"/> containing the operation result.
/// </remarks>
public record RegisterApplicationUserRequest(string Email, string Password, string ConfirmPassword)
    : IRequest<ApiResponse<object>>;