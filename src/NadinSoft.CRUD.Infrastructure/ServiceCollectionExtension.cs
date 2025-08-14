using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;
using NadinSoft.CRUD.Infrastructure.Data;
using NadinSoft.CRUD.Infrastructure.Repository;
using NadinSoft.CRUD.Infrastructure.Services.AuthService;

namespace NadinSoft.CRUD.Infrastructure;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddRepositories();

        services.AddCustomService();

        services.AddAuthenticationService(configuration);
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
    }

    private static void AddCustomService(this IServiceCollection service)
    {
        service.AddHttpContextAccessor();
        service.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        service.AddScoped<ICurrentUserService, CurrentUserService>();
        service.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        service.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(BuildConnectionStringFromEnvironment());
        });
    }

    private static void AddAuthenticationService(this IServiceCollection service,
        IConfigurationManager configurationManager)
    {
        service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configurationManager["Authentication:Issuer"],
                    ValidAudience = configurationManager["Authentication:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationManager["Authentication:Key"]!)),
                    ValidateIssuerSigningKey = true
                };
            });
    }

    private static string BuildConnectionStringFromEnvironment()
    {
        string? dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
        string? dbName = Environment.GetEnvironmentVariable("DB_NAME");
        string? dbUser = Environment.GetEnvironmentVariable("DB_USER");
        string? dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        string encrypt = Environment.GetEnvironmentVariable("DB_ENCRYPT") ?? "False";
        string trustCert = Environment.GetEnvironmentVariable("DB_TRUST_CERT") ?? "True";

        if (string.IsNullOrWhiteSpace(dbServer) ||
            string.IsNullOrWhiteSpace(dbName) ||
            string.IsNullOrWhiteSpace(dbUser) ||
            string.IsNullOrWhiteSpace(dbPassword))
        {
            throw new InvalidOperationException(
                "Invalid Connection String");
        }

        return
            $"Server={dbServer};Database={dbName};User Id={dbUser};Password={dbPassword};Encrypt={encrypt};TrustServerCertificate={trustCert};";
    }
}