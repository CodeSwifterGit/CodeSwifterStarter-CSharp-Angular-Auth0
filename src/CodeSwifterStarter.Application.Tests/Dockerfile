FROM mcr.microsoft.com/dotnet/sdk:5.0 AS restored
WORKDIR /src
COPY . .
RUN dotnet restore /src/src/CodeSwifterStarter.Web.Api/CodeSwifterStarter.Web.Api.csproj
RUN dotnet restore /src/src/CodeSwifterStarter.Application.Tests/CodeSwifterStarter.Application.Tests.csproj

FROM restored AS test
WORKDIR "/src/src/CodeSwifterStarter.Application.Tests"
RUN ASPNETCORE_ENVIRONMENT=Staging DOTNET_ENVIRONMENT=Staging dotnet test --blame-crash --blame-hang-dump-type mini --blame-hang-timeout 10m -l "console;verbosity=detailed" --configuration Jenkins
