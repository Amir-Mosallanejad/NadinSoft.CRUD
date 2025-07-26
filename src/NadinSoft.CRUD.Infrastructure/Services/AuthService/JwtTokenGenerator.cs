using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Infrastructure.Services.AuthService;

public class JwtTokenGenerator(IConfiguration config) : IJwtTokenGenerator
{
    public string GenerateToken(ApplicationUser user)
    {
        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Authentication:Key"]!));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: config["Authentication:Issuer"],
            audience: config["Authentication:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}