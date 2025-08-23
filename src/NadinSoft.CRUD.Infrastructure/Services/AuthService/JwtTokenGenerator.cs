using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NadinSoft.CRUD.Infrastructure.Services.AuthService;

/// <summary>
/// Provides functionality to generate JWT tokens for authenticated users.
/// </summary>
public class JwtTokenGenerator : IJwtTokenGenerator
{
    /// <summary>
    /// Provides access to the application configuration, such as settings from appsettings.json or environment variables.
    /// </summary>
    private readonly IConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtTokenGenerator"/> class.
    /// </summary>
    /// <param name="config">The <see cref="IConfiguration"/> used to read JWT settings.</param>
    public JwtTokenGenerator(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Generates a JWT token for the specified <see cref="ApplicationUser"/>.
    /// </summary>
    /// <param name="user">The authenticated user for whom the token is generated.</param>
    /// <returns>A signed JWT token as a <see cref="string"/>.</returns>
    public string GenerateToken(ApplicationUser user)
    {
        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Email, user.Email!),
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Authentication:Key"]!));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _config["Authentication:Issuer"],
            audience: _config["Authentication:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}