// <copyright file="LoginApplicationUserRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;

using MediatR;
using NadinSoft.CRUD.Application.Common.DTOs;

/// <summary>
/// Represents a request to log in an application user.
/// </summary>
/// <param name="Email">The email address of the user.</param>
/// <param name="Password">The password of the user.</param>
/// <remarks>
/// This request implements <see cref="IRequest{TResponse}"/> and expects an <see cref="ApiResponse{T}"/> containing a JWT string.
/// </remarks>
public record LoginApplicationUserRequest(string Email, string Password)
    : IRequest<ApiResponse<string>>;