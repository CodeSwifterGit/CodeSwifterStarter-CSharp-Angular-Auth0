FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-with-node
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -fsSL https://deb.nodesource.com/setup_15.x | bash -
RUN apt-get install -y nodejs
RUN npm install -g npm@latest

FROM build-with-node AS publish
WORKDIR /src
COPY . .
WORKDIR "/src/src/CodeSwifterStarter.Web.Api"
ARG GIT_COMMIT=0
ARG BUILD_NUMBER=0
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CodeSwifterStarter.Web.Api.csproj" -c $BUILD_CONFIGURATION  -p:VersionPrefix=1.0.0 -p:VersionSuffix=$BUILD_NUMBER -p:SourceRevisionId=$GIT_COMMIT -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV USE_HSTS false
ENTRYPOINT ["dotnet", "CodeSwifterStarter.Web.Api"]