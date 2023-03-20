FROM mcr.microsoft.com/dotnet/sdk:7.0 as base
ENV ASPNETCORE_URLS="http://+:5000"
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . ./
RUN dotnet restore
RUN dotnet build

FROM build as publish
RUN dotnet publish -c Release -o /publish

FROM base AS release
WORKDIR /app
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "Droits.dll"]
