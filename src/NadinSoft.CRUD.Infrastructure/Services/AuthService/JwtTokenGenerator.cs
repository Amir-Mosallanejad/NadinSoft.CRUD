// <copyright file="JwtTokenGenerator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Infrastructure.Services.AuthService;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Domain.Entities;

/// <summary>
/// Provides functionality to generate JWT tokens for authenticated users.
/// </summary>
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration config;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtTokenGenerator"/> class.
    /// </summary>
    /// <param name="config">The <see cref="IConfiguration"/> used to read JWT settings.</param>
    public JwtTokenGenerator(IConfiguration config)
    {
        this.config = config;
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
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["Authentication:Key"]!));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: this.config["Authentication:Issuer"],
            audience: this.config["Authentication:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}