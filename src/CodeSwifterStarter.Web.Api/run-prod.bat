echo .
echo Make sure to create WILDCARD LetsEncrypt certificate for testing purposes and put it into %USERPROFILE%\.cert\codeswifterstarter.com.pfx
echo .
SET ASPNETCORE_Kestrel__Certificates__Default__Password=X
set ASPNETCORE_Kestrel__Certificates__Default__Path=%USERPROFILE%\.cert\codeswifterstarter.com.pfx
SET DOTNET_ENVIRONMENT=Production
SET ASPNETCORE_ENVIRONMENT=Production
SET ASPNETCORE_URLS=https://+:443;http://+:80
dotnet run watch --no-launch-profile