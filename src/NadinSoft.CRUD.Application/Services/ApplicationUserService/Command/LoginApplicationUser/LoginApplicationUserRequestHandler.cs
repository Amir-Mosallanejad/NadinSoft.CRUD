using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;

/// <summary>
/// Handles login requests for <see cref="ApplicationUser"/> and generates a JWT token upon successful authentication.
/// </summary>
public class LoginApplicationUserRequestHandler(
    UserManager<ApplicationUser> userManager,
    IJwtTokenGenerator jwtTokenGenerator,
    ILogger<LoginApplicationUserRequestHandler> logger)
    : IRequestHandler<LoginApplicationUserRequest, ApiResponse<string>>
{
    /// <summary>
    /// Handles the <see cref="LoginApplicationUserRequest"/> by validating the user's credentials and generating a JWT token.
    /// </summary>
    /// <param name="request">The login request containing the user's email and password.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// An <see cref="ApiResponse{T}"/> containing the JWT token if login succeeds, or an error message if it fails.
    /// </returns>
    public async Task<ApiResponse<string>> Handle(
        LoginApplicationUserRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(request.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                return ApiResponse<string>.Fail("Invalid credentials.");
            }

            string token = jwtTokenGenerator.GenerateToken(user);
            return ApiResponse<string>.Success(token);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled error occurred while logging-in request.");

            return ApiResponse<string>.Fail("An unexpected error occurred.");
        }
    }
}