# syntax=docker/dockerfile:1

# Dockerfile for Buildline Platform API
# Summary:
# This Dockerfile builds and runs the Buildline ASP.NET Core Web Services backend.
# Description:
# The image uses a multi-stage .NET build. The first stage restores dependencies
# and publishes the application with the .NET SDK. The second stage runs only the
# published output on the smaller ASP.NET runtime image. The runtime container is
# prepared for Railway or any container host that supplies MySQL and JWT settings
# through environment variables.
# Version: 1.0
# Maintainer: RQLS Buildline Team

# Step 1: Build and publish the application using the .NET SDK.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# Set the working directory used by restore and publish commands.
WORKDIR /src

# Copy the project file first so Docker can cache dependency restore layers.
COPY Buildline/Platform/Buildline.Platform.csproj Buildline/Platform/

# Restore NuGet packages for the API project.
RUN dotnet restore Buildline/Platform/Buildline.Platform.csproj

# Copy the remaining source files, migrations, resources and seed data.
COPY . .

# Publish a Release build without a platform-specific app host executable.
RUN dotnet publish Buildline/Platform/Buildline.Platform.csproj \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

# Step 2: Create the runtime image using the ASP.NET Core runtime.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

# Set the application directory inside the runtime container.
WORKDIR /app

# Configure ASP.NET Core for container hosting on port 8080.
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Keep Swagger available for Sprint Review and deployment evidence.
ENV ENABLE_SWAGGER=true

# Copy only the published application output from the build stage.
COPY --from=build /app/publish .

# Expose the internal HTTP port used by Railway and local Docker runs.
EXPOSE 8080

# Step 3: Run the Buildline Platform API.
ENTRYPOINT ["dotnet", "Buildline.Platform.dll"]

# Note:
# The application expects runtime configuration from appsettings.*.json and the
# hosting provider environment. For production deployment, define:
# - ASPNETCORE_ENVIRONMENT: Must be Production for production settings.
# - ASPNETCORE_URLS: Internal binding URL, normally http://+:8080.
# - MYSQL_HOST: Database host name or address.
# - MYSQL_PORT: Database port, normally 3306.
# - MYSQL_DATABASE: Database name used by the API.
# - MYSQL_USER: Database username.
# - MYSQL_PASSWORD: Database password.
# - BUILDLINE_JWT_SECRET: JWT signing secret with at least 32 characters.
# - ENABLE_SWAGGER: Optional true/false flag to expose or hide Swagger UI.
