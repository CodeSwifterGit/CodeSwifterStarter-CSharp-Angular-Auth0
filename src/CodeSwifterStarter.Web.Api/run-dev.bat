echo .
echo Make sure to run "dotnet dev-certs https --trust" in order to create a self-signed certificate, if it is not yet created.
echo .
SET DOTNET_ENVIRONMENT=Development
SET ASPNETCORE_ENVIRONMENT=Development
SET ASPNETCORE_URLS=https://+:6220;http://+:6221
dotnet run watch --no-launch-profile