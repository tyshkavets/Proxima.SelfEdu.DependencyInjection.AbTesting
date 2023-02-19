# A/B Testing via ASP.NET Core DI.
Extensions for ASP.NET Core dependency injection allowing for easy A/B Testing.

## Usage

If you have multiple implementations for a service, you can register
them as an experiment. In this case, every time a new implementation is
constructed, one of the variants is selected at random.

```csharp
builder.Services.AddScopedExperiment<IMessageProvider>()
    .AddVariant<MessageProviderVariantA>()
    .AddVariant<MessageProviderVariantB>()
    .Finish();
```

One could also specify a weight (proportional probability) for a variant.
Variants that do not have weight specified are considered to have a weight of 1.0.
Managing weight values should allow you to set a certain ratio - for example, have a
baseline option and a variant that is provided for 10% of users.

```csharp
    .AddVariant<MessageProviderBaseline>(0.9)
    .AddVariant<MessageProviderExperimental>(0.1)
```

To monitor results, `Activity.Current`, if present, is annotated with `abtesting.variant.{serviceName}` tag.
You may find it useful for performance comparisons, or, for example, for comparing error rates.

## Versioning

We use semantic versioning for this package. If updating to next major version, please
check [Change Log](CHANGELOG.md) for breaking changes.

## Contributing

Please feel free to report any issues or submit pull requests via Github.