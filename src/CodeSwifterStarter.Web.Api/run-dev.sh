echo .
echo Make sure to run "dotnet dev-certs https --trust" in order to create a self-signed certificate, if it is not yet created.
echo .
export DOTNET_ENVIRONMENT=Development
export ASPNETCORE_ENVIRONMENT=Development
export ASPNETCORE_URLS=https://+:6220;http://+:6221
dotnet run watch --no-launch-profile