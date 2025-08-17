using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.CRUD.Application.Common.Behaviors;

namespace NadinSoft.CRUD.Application;

/// <summary>
/// Provides extension methods to register application-level services into the <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Registers application services including MediatR handlers, FluentValidation validators,
    /// pipeline behaviors, and AutoMapper profiles.
    /// </summary>
    /// <param name="service">The <see cref="IServiceCollection"/> to which services will be added.</param>
    public static void AddApplicationServices(this IServiceCollection service)
    {
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationProjectEntry).Assembly));

        service.AddValidatorsFromAssembly(typeof(ApplicationProjectEntry).Assembly);

        service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        service.AddAutoMapper(typeof(ApplicationProjectEntry).Assembly);
    }
}