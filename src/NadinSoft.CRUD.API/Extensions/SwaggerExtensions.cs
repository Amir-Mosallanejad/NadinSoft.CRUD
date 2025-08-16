// <copyright file="SwaggerExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.API.Extensions;

/// <summary>
/// Provides extension methods for configuring Swagger documentation and security.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Adds and configures custom Swagger generation with JWT authentication support.
    /// </summary>
    /// <param name="service">The service collection to which Swagger configuration is added.</param>
    /// <remarks>
    /// This method configures:
    /// <list type="bullet">
    /// <item><description>Swagger document metadata (title, version).</description></item>
    /// <item><description>Bearer token authentication in the Swagger UI.</description></item>
    /// <item><description>Security requirements for protected endpoints.</description></item>
    /// </list>
    /// </remarks>
    public static void AddCustomSwaggerGen(this IServiceCollection service)
    {
        service.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "ProductApp API", Version = "v1" });

            options.AddSecurityDefinition(
                "Bearer",
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter your JWT token like: **Bearer YOUR_TOKEN_HERE**",
                });

            options.AddSecurityRequirement(
                new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                });
        });
    }
}