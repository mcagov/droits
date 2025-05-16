# Droits Backoffice

Welcome to the `droits-backoffice` application! This repository contains the backoffice services and utilities for the Droits platform.

## Getting Started

Before diving into the application, there are a few prerequisites you need to set up for local development:

### Development Prerequisites

1. **Docker**: Ensure you have Docker installed. If not, [download Docker](https://www.docker.com/products/docker-desktop) for your specific OS.

2. **.NET SDK**: This application is developed using .NET. Ensure you have the [.NET SDK](https://dotnet.microsoft.com/download) installed.

3. **Development Certificate**: To run the application locally with HTTPS, you'll need a development certificate. Follow the instructions below to set one up:
    ```bash
    dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p password
    ```
4. Create a `src/appsettings.json` file and populate it with the contents of the "Backoffice Local- appsettings.json" secret from 1Password.

This command will create a local development certificate for HTTPS and store it in the specified location.

### Running the Application

Navigate to the root of the repository, and use the following command to build and start the Docker containers for the application:

```bash
docker-compose up backoffice --build
```

This command will build the application's Docker images and start the necessary containers. After the process is complete, the `droits-backoffice` application should be accessible via your web browser.

## Testing

### Run the unit tests

```shell
# From the backoffice directory...

dotnet test --filter FullyQualifiedName\~UnitTests
```

### Mutation testing

We use [Stryker.Net](https://stryker-mutator.io/docs/stryker-net/introduction/) as a tool to help us understand how much we can trust our unit tests.

Every mutation that survives is a line of code that we can change without it being picked up by our unit tests.

To run the mutation tests:

```shell
# From the webapp directory...

dotnet stryker
```

This will take a while, so you are not going to be running it after every commit.

Once it completes, there should be an HTML report in `reports/mutation/mutation.html`.

> We need to get `"test-case-filter": "FullyQualifiedName~UnitTests",` working in the config. See [this issue](https://github.com/stryker-mutator/stryker-net/issues/3242).
