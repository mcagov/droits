FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS release
WORKDIR /app
COPY --from=build /publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "Droits.dll"]
