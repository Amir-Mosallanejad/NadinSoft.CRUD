using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
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
using NadinSoft.CRUD.Infrastructure.Services.Localization;
using System.Globalization;
using System.Text;

namespace NadinSoft.CRUD.Infrastructure;

/// <summary>
/// Provides extension methods to register infrastructure services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Registers all infrastructure services including repositories, custom services, and authentication.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing application settings.</param>
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddUnitOfWork();
        services.AddCustomService();
        services.AddAuthenticationService(configuration);
    }

    /// <summary>
    /// Registers Unit of Work.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add repositories to.</param>
    private static void AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    /// <summary>
    /// Registers custom services such as JWT generator, current user service, Identity, and the database context.
    /// </summary>
    /// <param name="service">The <see cref="IServiceCollection"/> to add services to.</param>
    private static void AddCustomService(this IServiceCollection service)
    {
        service.AddHttpContextAccessor();
        service.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        service.AddScoped<ICurrentUserService, CurrentUserService>();
        service.AddScoped<ILocalizationService, LocalizationService>();
        service.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        service.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(BuildConnectionStringFromEnvironment());
        });

        service.AddLocalization();
        CultureInfo[] supportedCultures = new[]
        {
            new CultureInfo("en-US"), new CultureInfo("fa-IR"),
        };

        service.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("fa-IR");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            options.RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new QueryStringRequestCultureProvider(),
            };
        });
    }

    /// <summary>
    /// Configures JWT authentication for the application.
    /// </summary>
    /// <param name="service">The <see cref="IServiceCollection"/> to add authentication to.</param>
    /// <param name="configurationManager">The <see cref="IConfiguration"/> providing authentication settings.</param>
    private static void AddAuthenticationService(
        this IServiceCollection service,
        IConfiguration configurationManager)
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
                    ValidateIssuerSigningKey = true,
                };
            });
    }

    /// <summary>
    /// Builds the SQL Server connection string from environment variables.
    /// </summary>
    /// <returns>The constructed connection string.</returns>
    /// <exception cref="InvalidOperationException">Thrown if any required environment variable is missing or empty.</exception>
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
            throw new InvalidOperationException("Invalid Connection String");
        }

        return
            $"Server={dbServer};Database={dbName};User Id={dbUser};Password={dbPassword};Encrypt={encrypt};TrustServerCertificate={trustCert};";
    }
}