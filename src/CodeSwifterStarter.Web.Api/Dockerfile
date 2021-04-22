FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS restored
WORKDIR /src
COPY . .
RUN dotnet restore /src/src/CodeSwifterStarter.Web.Api/CodeSwifterStarter.Web.Api.csproj
RUN dotnet restore /src/src/CodeSwifterStarter.Application.Tests/CodeSwifterStarter.Application.Tests.csproj

FROM restored AS publish
ARG GIT_COMMIT=0
ARG BUILD_NUMBER=0
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/src/CodeSwifterStarter.Web.Api"
RUN dotnet publish "CodeSwifterStarter.Web.Api.csproj" -c $BUILD_CONFIGURATION  -p:VersionPrefix=1.0.0 -p:VersionSuffix=$BUILD_NUMBER -p:SourceRevisionId=$GIT_COMMIT -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV USE_HSTS false
ENTRYPOINT ["dotnet", "CodeSwifterStarter.Web.Api"]