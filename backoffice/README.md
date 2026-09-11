# Droits Backoffice

Welcome to the `droits-backoffice` application! This repository contains the backoffice services and utilities for the Droits platform.

## Local development

Follow [the instructions in the root README to get started](../README.md#local-development).

## Testing

### Run the unit tests

```shell
# From the backoffice directory...

dotnet test --filter FullyQualifiedName~UnitTests

### Run the integration tests

```shell
# From the backoffice directory...

dotnet test --filter FullyQualifiedName~IntegrationTests
```

### Mutation testing

We use [Stryker.Net](https://stryker-mutator.io/docs/stryker-net/introduction/) as a tool to help us understand how much we can trust our unit tests.

Every mutation that survives is a line of code that we can change without it being picked up by our unit tests.

To run the mutation tests:

```shell
# From the backoffice directory...

dotnet stryker
```

This will take a while, so you are not going to be running it after every commit.

Once it completes, there should be an HTML report in `reports/mutation/mutation.html`.

> We need to get `"test-case-filter": "FullyQualifiedName~UnitTests",` working in the config. See [this issue](https://github.com/stryker-mutator/stryker-net/issues/3242).
