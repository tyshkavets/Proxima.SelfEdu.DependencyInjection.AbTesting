using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Proxima.SelfEdu.DependencyInjection.AbTesting;

public class ExperimentContext<TService> where TService : class
{
    private readonly IServiceCollection _services;
    private readonly ServiceLifetime _lifetime;
    private readonly IList<(double Weight, Type Type)> _implementationTypes = new List<(double Weight, Type Type)>();

    public ExperimentContext(IServiceCollection services, ServiceLifetime lifetime)
    {
        _services = services;
        _lifetime = lifetime;
    }

    public ExperimentContext<TService> AddVariant<TImplementation>(double weight = 1.0)
        where TImplementation : class, TService
    {
        _implementationTypes.Add((weight, typeof(TImplementation)));
        return this;
    }

    public void Finish()
    {
        if (_implementationTypes.Count == 0)
        {
            return;
        }

        var container = new WeightedContainer<Type>(_implementationTypes, new Random());

        var descriptor = new ServiceDescriptor(
            typeof(TService),
            sp =>
            {
                var result = container.NextRandom();

                if (Activity.Current != null)
                {
                    var serviceName = typeof(TService).Name.ToLower();
                    var implementationName = result.Name.ToLower();
                    Activity.Current.AddTag($"abtesting.variant.{serviceName}", implementationName);
                }

                return (TService)ActivatorUtilities.CreateInstance(sp, result);
            },
            _lifetime);

        _services.Add(descriptor);
    }
}