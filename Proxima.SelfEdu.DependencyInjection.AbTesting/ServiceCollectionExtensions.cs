using Microsoft.Extensions.DependencyInjection;

namespace Proxima.SelfEdu.DependencyInjection.AbTesting;

public static class ServiceCollectionExtensions
{
    public static ExperimentContext<TService> AddScopedExperiment<TService>(this IServiceCollection serviceCollection)
        where TService : class
    {
        return new ExperimentContext<TService>(serviceCollection);
    }
}