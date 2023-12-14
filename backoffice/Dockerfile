FROM mcr.microsoft.com/dotnet/sdk:7.0 as base
ENV ASPNETCORE_URLS="http://+:5000"
ENV NO_PROXY=169.254.169.254,169.254.170.2
ENV AWS_REGION="eu-west-2"

EXPOSE 5000

# Timezone setup
RUN ln -snf /usr/share/zoneinfo/Europe/London /etc/localtime && echo "Europe/London" > /etc/timezone

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

COPY . ./

WORKDIR /app/src


# Node setup
RUN apt-get update && \
    apt-get install -y curl && \
    curl -sL https://deb.nodesource.com/setup_14.x | bash - && \
    apt-get install -y nodejs && \
    npm install --production

# Build .NET app
RUN dotnet restore
RUN dotnet build


FROM build as publish
RUN dotnet publish -c Release -o /publish

FROM base AS release
WORKDIR /app
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "Droits.dll"]
