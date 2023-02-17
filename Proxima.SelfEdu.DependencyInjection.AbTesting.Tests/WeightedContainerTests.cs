namespace Proxima.SelfEdu.DependencyInjection.AbTesting.Tests;

public class WeightedContainerTests
{
    [Test]
    public void ReturnValue_IsOneOfTheValuesPassed()
    {
        var options = new WeightedContainer<int>(new []
        {
            (1.0, 4),
            (1, 5),
            (1, 6),
            (1, 7)
        }, Random.Shared);

        var result = options.NextRandom();
        
        Assert.That(result is >= 4 and <= 7, Is.True);
    }

    [Test]
    public void WithPredeterminedRng_ReturnsValueFromExpectedBucket()
    {
        // It will always generate 0.249 as the first double value.
        var rng = new Random(1);
        
        var options = new WeightedContainer<int>(new []
        {
            (1.0, 4), // this value should occupy a stretch of [0; 0.25] for the .NextDouble() of RNG.
            (1, 5),
            (1, 6),
            (1, 7)
        }, rng);
        
        Assert.That(options.NextRandom(), Is.EqualTo(4));
    }

    [Test]
    public void WithPredeterminedRng_AndUnequalWeights_ReturnsValueFromExpectedBucket()
    {
        // It will always generate 0.249 as the first double value.
        var rng = new Random(1);
        
        var options = new WeightedContainer<int>(new []
        {
            (1.0, 4),
            (1000, 5),
            (1, 6),
            (1, 7)
        }, rng);
        
        Assert.That(options.NextRandom(), Is.EqualTo(5));
    }
}