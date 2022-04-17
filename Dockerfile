FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Prevent 'Warning: apt-key output should not be parsed (stdout is not a terminal)'
ENV APT_KEY_DONT_WARN_ON_DANGEROUS_USAGE=1
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=https://+:5000
EXPOSE 5001
EXPOSE 5000
RUN dotnet dev-certs https

COPY . .

#Build project
RUN dotnet publish HeromaVgrIcalSubscription.sln -c Release -o out

ENTRYPOINT ["dotnet", "/app/out/HeromaVgrIcalSubscription.dll"]
