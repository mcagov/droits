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

This command will create a local development certificate for HTTPS and store it in the specified location.

### Running the Application

Navigate to the root of the repository, and use the following command to build and start the Docker containers for the application:

```bash
docker-compose up backoffice --build
```

This command will build the application's Docker images and start the necessary containers. After the process is complete, the `droits-backoffice` application should be accessible via your web browser.
