FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

# Build and publish
COPY . .
RUN dotnet publish HeromaVgrIcalSubscription.sln -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "HeromaVgrIcalSubscription.dll"]
