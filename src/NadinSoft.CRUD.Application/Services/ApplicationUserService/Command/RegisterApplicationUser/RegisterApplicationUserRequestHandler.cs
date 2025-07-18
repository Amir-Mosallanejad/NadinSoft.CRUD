using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Application.Common.DTOs;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;

public class RegisterApplicationUserRequestHandler(
    UserManager<ApplicationUser> userManager,
    IMapper mapper,
    ILogger<RegisterApplicationUserRequestHandler> logger)
    : IRequestHandler<RegisterApplicationUserRequest, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(RegisterApplicationUserRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            ApplicationUser? existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                logger.LogInformation("Registration attempt failed: User with email {Email} already exists.",
                    request.Email);
                return ApiResponse<object>.Fail("User with this email already exists.");
            }

            ApplicationUser user = mapper.Map<ApplicationUser>(request);
            IdentityResult result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                string errorMessages = string.Join(" | ", result.Errors.Select(e => e.Description));
                logger.LogWarning("User registration failed for email {Email}: {Errors}", request.Email, errorMessages);
                return ApiResponse<object>.Fail(errorMessages);
            }

            return ApiResponse<object>.Success(new object());
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled error occurred while registering request.");

            return ApiResponse<object>.Fail("An unexpected error occurred.");
        }
    }
}