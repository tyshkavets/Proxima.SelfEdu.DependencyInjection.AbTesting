using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Proxima.SelfEdu.DependencyInjection.AbTesting;

public class ExperimentContext<TService> where TService : class
{
    private readonly IServiceCollection _serviceCollection;
    private readonly IList<Type> _implementationTypes = new List<Type>();

    public ExperimentContext(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
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

        _serviceCollection.AddScoped(sp =>
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
        });
    }
}