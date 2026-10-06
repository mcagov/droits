FROM mcr.microsoft.com/dotnet/sdk:9.0 as base
ENV ASPNETCORE_URLS="http://+:5000"
ENV NO_PROXY=169.254.169.254,169.254.170.2
ENV AWS_REGION="eu-west-2"

EXPOSE 5000

# Timezone setup
RUN ln -snf /usr/share/zoneinfo/Europe/London /etc/localtime && echo "Europe/London" > /etc/timezone

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY . ./

WORKDIR /app/src


# Node setup
RUN RUN set -uex; \
    apt-get update; \
    apt-get install -y ca-certificates curl gnupg; \
    mkdir -p /etc/apt/keyrings; \
    curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key \
     | gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg; \
    NODE_MAJOR=18; \
    echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_$NODE_MAJOR.x nodistro main" \
     > /etc/apt/sources.list.d/nodesource.list; \
    apt-get -qy update; \
    apt-get -qy install nodejs && \
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
