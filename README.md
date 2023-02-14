# A/B Testing via ASP.NET Core DI.
Extensions for ASP.NET Core dependency injection allowing for easy A/B Testing.

## Usage

If you have multiple implementations for a service, you can register
them as an experiment. In this case, every time a new implementation is
constructed, one of the variants is selected at random.

To monitor results, `Activity.Current` is annotated with `abtesting.variant.{serviceName}` tag.
You may find it useful for performance comparisons, or, for example, for comparing error rates.

```csharp
builder.Services.AddScopedExperiment<IMessageProvider>()
    .AddVariant<MessageProviderVariantA>()
    .AddVariant<MessageProviderVariantB>()
    .Finish();
```

## Versioning

We use semantic versioning for this package. If updating to next major version, please
check [Change Log](CHANGELOG.md) for breaking changes.

## Contributing

Please feel free to report any issues or submit pull requests via Github.