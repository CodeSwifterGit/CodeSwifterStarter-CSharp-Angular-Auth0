FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-with-node
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -fsSL https://deb.nodesource.com/setup_15.x | bash -
RUN apt-get install -y nodejs
RUN npm install -g npm@latest

FROM build-with-node AS source
WORKDIR /src
COPY . .
WORKDIR "/src/src/CodeSwifterStarter.Application.Tests"
RUN ASPNETCORE_ENVIRONMENT=Staging DOTNET_ENVIRONMENT=Staging dotnet test --blame-crash --blame-hang-dump-type mini --blame-hang-timeout 10m -l "console;verbosity=detailed" --configuration Jenkins
