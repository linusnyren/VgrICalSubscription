FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# Prevent 'Warning: apt-key output should not be parsed (stdout is not a terminal)'
ENV APT_KEY_DONT_WARN_ON_DANGEROUS_USAGE=1
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=https://+:5000
EXPOSE 5001
EXPOSE 5000

#Get geckodriver for Selenium to use
RUN wget https://github.com/mozilla/geckodriver/releases/download/v0.29.1/geckodriver-v0.29.1-linux32.tar.gz
RUN tar -xvzf geckodriver*
RUN chmod +x geckodriver

#Install Git
RUN apt-get install -yq git

#Clone project from github
RUN git clone https://github.com/linusnyren/VgrICalSubscription.git

#Change path to geckodriver in appsettings.json
RUN sed -i 's+/Users/LinusNyren/Downloads+/app+g' VgrICalSubscription/HeromaVgrIcalSubscription/appsettings.json

RUN dotnet dev-certs https

#Build project
RUN dotnet publish VgrICalSubscription/HeromaVgrIcalSubscription.sln -c Debug
ENTRYPOINT ["dotnet", "/app/VgrICalSubscription/HeromaVgrIcalSubscription/bin/Debug/netcoreapp3.1/HeromaVgrIcalSubscription.dll"]
