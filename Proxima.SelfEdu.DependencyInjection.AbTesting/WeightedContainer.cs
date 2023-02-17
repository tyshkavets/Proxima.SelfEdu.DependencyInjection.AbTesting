namespace Proxima.SelfEdu.DependencyInjection.AbTesting;

public class WeightedContainer<T>
{
    private readonly IList<(double Weight, T Object)> _weightedObjects;
    private readonly Random _rng;
    private readonly double _weightSum;

    public WeightedContainer(IEnumerable<(double Weight, T Object)> objects)
        : this(objects, new Random())
    {
    }
    
    public WeightedContainer(IEnumerable<(double Weight, T Object)> objects, Random rng)
    {
        _rng = rng;
        _weightedObjects = new List<(double, T)>();
        double currentCumulative = 0;
        
        foreach (var weightedObject in objects)
        {
            if (weightedObject.Weight <= 0)
            {
                throw new ArgumentException(
                    $"Cannot incorporate weighted object with a non-positive weight: {weightedObject.Weight}.");
            }

            currentCumulative += weightedObject.Weight;
            _weightedObjects.Add((currentCumulative, weightedObject.Object));
        }

        _weightSum = currentCumulative;
    }

    public T NextRandom()
    {
        var weight = _rng.NextDouble() * _weightSum;
        (double Weight, T Object) previousObject = default;
        var first = true;

        foreach (var weightedObjectPair in _weightedObjects)
        {
            if (weight < weightedObjectPair.Weight && (first || weight > previousObject.Weight))
            {
                return weightedObjectPair.Object;
            }

            first = false;
            previousObject = weightedObjectPair;
        }

        return _weightedObjects.Last().Object;
    }
}