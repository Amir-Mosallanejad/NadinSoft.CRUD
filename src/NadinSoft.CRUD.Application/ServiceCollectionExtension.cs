using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.CRUD.Application.Common.Behaviors;

namespace NadinSoft.CRUD.Application;

public static class ServiceCollectionExtension
{
    public static void AddApplicationServices(this IServiceCollection service)
    {
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationProjectEntry).Assembly));
        service.AddValidatorsFromAssembly(typeof(ApplicationProjectEntry).Assembly);
        service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        service.AddAutoMapper(typeof(ApplicationProjectEntry).Assembly);
    }
}