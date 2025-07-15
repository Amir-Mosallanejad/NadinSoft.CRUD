using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NadinSoft.CRUD.Application;
using NadinSoft.CRUD.Application.Common.Behaviors;
using NadinSoft.CRUD.Application.Common.Interfaces;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;
using NadinSoft.CRUD.Infrastructure.Data;
using NadinSoft.CRUD.Infrastructure.Repository;
using NadinSoft.CRUD.Infrastructure.Services.AuthService;

namespace NadinSoft.CRUD.Infrastructure;

public static class ServiceCollectionExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
    }

    public static void AddCustomService(this IServiceCollection service)
    {
        service.AddHttpContextAccessor();
        service.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        service.AddScoped<ICurrentUserService, CurrentUserService>();
        service.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        service.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(Environment.GetEnvironmentVariable("CURRENT_DB_CONNECTION_STRING"));
        });
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationProjectEntry).Assembly));
        service.AddValidatorsFromAssembly(typeof(ApplicationProjectEntry).Assembly);
        service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        service.AddAutoMapper(typeof(ApplicationProjectEntry).Assembly);
    }

    public static void AddAuthenticationService(this IServiceCollection service,
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

    public static void AddCustomSwaggerGen(this IServiceCollection service)
    {
        service.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "ProductApp API", Version = "v1" });

            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter your JWT token like: **Bearer YOUR_TOKEN_HERE**"
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });
    }
}