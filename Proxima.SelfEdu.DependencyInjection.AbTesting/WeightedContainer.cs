namespace Proxima.SelfEdu.DependencyInjection.AbTesting;

/// <summary>
/// a container that is capable of returning one of the objects it contains
/// at random according to assigned weights (probabilities).
/// Container is immutable by itself and does not support operations that change its internal state,
/// other than the internal state of the random number generator.
/// </summary>
/// <typeparam name="T">Type of the objects inside the container.</typeparam>
public sealed class WeightedContainer<T>
{
    private readonly IList<(double Weight, T Object)> _weightedObjects;
    private readonly Random _rng;
    private readonly double _weightSum;

    /// <summary>
    /// Constructs the instance of weighted container with objects it should contain.
    /// Random number generator will be created automatically.
    /// </summary>
    /// <param name="objects">Objects to store in the container.</param>
    /// <exception cref="ArgumentException">Thrown if weight is non-positive.</exception>
    public WeightedContainer(IEnumerable<(double Weight, T Object)> objects)
        : this(objects, new Random())
    {
    }

    /// <summary>
    /// Constructs the instance of weighted container with objects it should contain.
    /// </summary>
    /// <param name="objects">Objects to store in the container.</param>
    /// <param name="rng">Random number generator tp generate randomness required to pick items to return.</param>
    /// <exception cref="ArgumentException">Thrown if weight is non-positive.</exception>
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

    /// <summary>
    /// Returns one of the registered objects at random, with probability of each object being returned corresponding
    /// to the weight assigned to this object during registration.
    /// </summary>
    /// <returns>One of the values </returns>
    public T NextRandom()
    {
        var weight = _rng.NextDouble() * _weightSum;
        (double Weight, T Object) previousObject = default;
        var first = true;

        // As weight values are cumulative and therefore sorted, binary search would be more practical
        // for larger collections. However, for the intended purpose of this library (to use in A/B testing),
        // expected amount of variants to handle is within low single digits, so we'll go with simple list iteration.
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