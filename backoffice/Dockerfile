FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . ./
RUN dotnet restore
RUN dotnet build

FROM build as publish
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS release
WORKDIR /app
COPY --from=publish /publish .
EXPOSE 5001
ENTRYPOINT ["dotnet", "Droits.dll"]
