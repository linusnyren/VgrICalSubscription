FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# Prevent 'Warning: apt-key output should not be parsed (stdout is not a terminal)'
ENV APT_KEY_DONT_WARN_ON_DANGEROUS_USAGE=1
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=https://+:5001
EXPOSE 5001
EXPOSE 5000

# Set the Chrome repo.
RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - \
    && echo "deb http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list
# Install Chrome.
RUN apt-get update && apt-get -y install google-chrome-stable

#Get geckodriver for Selenium to use
RUN wget https://chromedriver.storage.googleapis.com/91.0.4472.19/chromedriver_linux64.zip
RUN apt-get install -y unzip
RUN unzip chromedriver_linux64.zip
RUN chmod +x chromedriver

#Install Git
RUN apt-get install -yq git

#Clone project from github
RUN git clone https://github.com/linusnyren/VgrICalSubscription.git

#Change path to geckodriver in appsettings.json
RUN sed -i 's+/Users/LinusNyren/Downloads+/app+g' VgrICalSubscription/HeromaVgrIcalSubscription/appsettings.json

#Change locale since docker is using the english website version
RUN sed -i 's+Swedish+English+g' VgrICalSubscription/HeromaVgrIcalSubscription/appsettings.json

#Build project
RUN dotnet publish VgrICalSubscription/HeromaVgrIcalSubscription.sln -c Debug
ENTRYPOINT ["dotnet", "/app/VgrICalSubscription/HeromaVgrIcalSubscription/bin/Debug/netcoreapp3.1/HeromaVgrIcalSubscription.dll"]
