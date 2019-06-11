FROM mcr.microsoft.com/dotnet/framework/aspnet:4.7.2-windowsservercore-1803
EXPOSE 80

# copy BlogEngine.NET
WORKDIR /inetpub/wwwroot
COPY dist/. .

# configure
COPY init.ps1 c:/
RUN powershell.exe c:\init.ps1