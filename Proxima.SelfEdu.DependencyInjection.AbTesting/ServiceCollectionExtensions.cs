using Microsoft.Extensions.DependencyInjection;

namespace Proxima.SelfEdu.DependencyInjection.AbTesting;

public static class ServiceCollectionExtensions
{
    public static ExperimentContext<TService> AddScopedExperiment<TService>(this IServiceCollection services)
        where TService : class
        => new(services, ServiceLifetime.Scoped);

    public static ExperimentContext<TService> AddTransientExperiment<TService>(this IServiceCollection services)
        where TService : class
        => new(services, ServiceLifetime.Transient);

    public static ExperimentContext<TService> AddSingleton<TService>(this IServiceCollection services)
        where TService : class
        => new(services, ServiceLifetime.Singleton);

    private static ExperimentContext<TService> Add<TService>(IServiceCollection services, ServiceLifetime lifetime)
        where TService : class
        => new(services, lifetime);
}