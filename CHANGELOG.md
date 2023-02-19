# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2023-02-19

### Changed

- **BREAKING**: `Activity.Current` now receives a tag rather than a baggage, meaning that it does not propagate to child activities.

### Fixed

- Documentation amended and improved.

## [1.2.0] - 2023-02-18

### Added

- Added optional `weight` parameter to `ExperimentContext.AddVariant<TImplementation()`.

## [1.1.0] - 2023-02-14

### Added

- Added `.AddTransientExperiment` and `.AddSingletonExperiment`.

## [1.0.0] - 2023-02-12

### Added

- Added `.AddScopedExperiment` extension method to `IServiceCollection`.