# Multi-stage build for QuantityMeasurementApp.Api
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first for better layer caching during restore
COPY QuantityMeasurementApp.Api/QuantityMeasurementApp.Api.csproj QuantityMeasurementApp.Api/
COPY QuantityMeasurementApp.Business/QuantityMeasurementApp.Business.csproj QuantityMeasurementApp.Business/
COPY QuantityMeasurementApp.Models/QuantityMeasurementApp.Models.csproj QuantityMeasurementApp.Models/
COPY QuantityMeasurementApp.Repository/QuantityMeasurementApp.Repository.csproj QuantityMeasurementApp.Repository/

RUN dotnet restore QuantityMeasurementApp.Api/QuantityMeasurementApp.Api.csproj

# Copy source and publish
COPY . .
WORKDIR /src/QuantityMeasurementApp.Api
RUN dotnet publish QuantityMeasurementApp.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "QuantityMeasurementApp.Api.dll"]
