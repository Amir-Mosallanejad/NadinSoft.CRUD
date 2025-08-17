using Microsoft.AspNetCore.Http;
using NadinSoft.CRUD.Application.Common.Interfaces;
using System.Security.Claims;

namespace NadinSoft.CRUD.Infrastructure.Services.AuthService;

/// <summary>
/// Provides information about the currently authenticated user.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    /// <summary>
    /// Provides access to the current HTTP context.
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentUserService"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> used to access the current HTTP context.</param>
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the user identifier of the currently authenticated user.
    /// Returns <c>null</c> if no user is authenticated.
    /// </summary>
    public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}