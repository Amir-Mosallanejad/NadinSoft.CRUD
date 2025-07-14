using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.CRUD.Application;
using NadinSoft.CRUD.Application.Common.Behaviors;
using NadinSoft.CRUD.Application.Common.Mappings;
using NadinSoft.CRUD.Domain.Repository;
using NadinSoft.CRUD.Infrastructure.Data;
using NadinSoft.CRUD.Infrastructure.Repository;

namespace NadinSoft.CRUD.Infrastructure;

public static class ServiceCollectionExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
    }

    public static void AddCustomService(this IServiceCollection service, IConfigurationManager configuration)
    {
        service.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("Default"));
        });
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationProjectEntry).Assembly));
        service.AddValidatorsFromAssembly(typeof(ApplicationProjectEntry).Assembly);
        service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        service.AddAutoMapper(typeof(ProductProfile));
    }
}