using Application.Behaviours;
using Application.Interfaces.Validations;

using Microsoft.Extensions.DependencyInjection;

using System.Linq;
using System.Reflection;

namespace Application;

public static class ServiceExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddScoped<IRequestPipelineExecutor, ValidationBehaviour>();
        services.AddRequestValidators(Assembly.GetExecutingAssembly());
        services.AddRequestHandlers(Assembly.GetExecutingAssembly());
    }

    private static IServiceCollection AddRequestHandlers(this IServiceCollection services, Assembly assembly)
    {
        var validatorInterfaceType = typeof(IRequestHandler<,>);

        var types = assembly.DefinedTypes
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .Select(type => new
            {
                Implementation = type.AsType(),
                Service = type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorInterfaceType)
            })
            .Where(x => x.Service != null);

        foreach (var typePair in types)
        {
            services.AddTransient(typePair.Service, typePair.Implementation);
        }

        return services;
    }

    private static IServiceCollection AddRequestValidators(this IServiceCollection services, Assembly assembly)
    {
        var validatorInterfaceType = typeof(IRequestValidator<>);

        var types = assembly.DefinedTypes
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .Select(type => new
            {
                Implementation = type.AsType(),
                Service = type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorInterfaceType)
            })
            .Where(x => x.Service != null);

        foreach (var typePair in types)
        {
            services.AddTransient(typePair.Service, typePair.Implementation);
        }

        return services;
    }
}