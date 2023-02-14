using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Proxima.SelfEdu.DependencyInjection.AbTesting;

public class ExperimentContext<TService> where TService : class
{
    private readonly IServiceCollection _services;
    private readonly ServiceLifetime _lifetime;
    private readonly IList<Type> _implementationTypes = new List<Type>();

    public ExperimentContext(IServiceCollection services, ServiceLifetime lifetime)
    {
        _services = services;
        _lifetime = lifetime;
    }

    public ExperimentContext<TService> AddVariant<TImplementation>() where TImplementation : class, TService
    {
        _implementationTypes.Add(typeof(TImplementation));
        return this;
    }

    public void Finish()
    {
        if (_implementationTypes.Count == 0)
        {
            return;
        }

        var descriptor = new ServiceDescriptor(
            typeof(TService),
            sp =>
            {
                var random = new Random();
                var pick = random.Next(_implementationTypes.Count);

                if (Activity.Current != null)
                {
                    var serviceName = typeof(TService).Name.ToLower();
                    var implementationName = _implementationTypes[pick].Name.ToLower();
                    Activity.Current.AddBaggage($"abtesting.variant.{serviceName}", implementationName);
                }

                return (TService)ActivatorUtilities.CreateInstance(sp, _implementationTypes[pick]);
            },
            _lifetime);

        _services.Add(descriptor);
    }
}