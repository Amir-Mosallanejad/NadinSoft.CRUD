using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;

public class LoginApplicationUserRequestHandler(
    UserManager<ApplicationUser> userManager,
    IJwtTokenGenerator jwtTokenGenerator,
    IMapper mapper,
    ILogger<LoginApplicationUserRequestHandler> logger)
    : IRequestHandler<LoginApplicationUserRequest, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(LoginApplicationUserRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(request.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                return new("Invalid credentials.");
            }


            string token = jwtTokenGenerator.GenerateToken(user);
            return new(token);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled error occurred while logging-in request.");

            return new("An unexpected error occurred.");
        }
    }
}