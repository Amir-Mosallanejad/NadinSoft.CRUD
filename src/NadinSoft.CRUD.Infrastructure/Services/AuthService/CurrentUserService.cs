using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NadinSoft.CRUD.Application.Common.Interfaces;

namespace NadinSoft.CRUD.Infrastructure.Services.AuthService;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId =>
        _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}