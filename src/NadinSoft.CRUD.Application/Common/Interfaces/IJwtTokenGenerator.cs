using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Application.Common.Interfaces;

/// <summary>
/// Defines a service responsible for generating JSON Web Tokens (JWT) for users.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generates a JWT for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the token is generated.</param>
    /// <returns>A string containing the generated JWT.</returns>
    string GenerateToken(ApplicationUser user);
}