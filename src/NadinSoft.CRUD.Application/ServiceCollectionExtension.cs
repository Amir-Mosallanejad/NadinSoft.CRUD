// <copyright file="ServiceCollectionExtension.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Application;

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.CRUD.Application.Common.Behaviors;

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
        // Register MediatR handlers from the assembly containing ApplicationProjectEntry
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationProjectEntry).Assembly));

        // Register FluentValidation validators from the same assembly
        service.AddValidatorsFromAssembly(typeof(ApplicationProjectEntry).Assembly);

        // Register the validation pipeline behavior
        service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Register AutoMapper profiles from the assembly
        service.AddAutoMapper(typeof(ApplicationProjectEntry).Assembly);
    }
}