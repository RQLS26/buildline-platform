# syntax=docker/dockerfile:1

################################################################################
# Buildline Platform API - Production Dockerfile
# ------------------------------------------------------------------------------
# Purpose
#   Builds and runs the ASP.NET Core Web Services backend used by the Buildline
#   Sprint 3 delivery. The image is designed for Railway and any container host
#   that provides environment variables for MySQL and JWT configuration.
#
# Design
#   This file uses a two-stage Docker build:
#   1. build   : restores NuGet packages and publishes the ASP.NET Core project.
#   2. runtime : runs only the published output on the smaller ASP.NET runtime.
#
# Why two stages?
#   The SDK image contains compilers, restore tooling and build-time assets that
#   are unnecessary at runtime. Copying only /app/publish into the final image
#   reduces the deployment surface and keeps the container closer to production
#   hosting expectations.
#
# Runtime contract
#   The application listens on port 8080 inside the container. Railway maps its
#   public HTTPS edge to this internal HTTP port, so TLS termination is handled
#   by Railway and the app itself binds to HTTP.
#
# Required environment variables in Railway
#   ASPNETCORE_ENVIRONMENT = Production
#   ASPNETCORE_URLS        = http://+:8080
#   MYSQL_HOST             = MySQL service host from Railway Connect tab
#   MYSQL_PORT             = 3306
#   MYSQL_DATABASE         = MySQL database name from Railway Connect tab
#   MYSQL_USER             = MySQL username from Railway Connect tab
#   MYSQL_PASSWORD         = MySQL password from Railway Connect tab
#   BUILDLINE_JWT_SECRET   = Long signing key, at least 32 characters
#
# Optional environment variables
#   ENABLE_SWAGGER         = true/false to override the Production Swagger setting
#
# Database startup behavior
#   Program.cs applies EF Core migrations when possible and then runs the JSON
#   demo-data seeder. If the database already contains a legacy EnsureCreated
#   schema with records, startup preserves the data and skips destructive work.
################################################################################

################################################################################
# Stage 1: Build and publish
# ------------------------------------------------------------------------------
# The .NET SDK image is intentionally used only for compilation. Pinning the
# major version to 10.0 keeps the container aligned with the project target
# framework while still receiving patch updates from the Microsoft image stream.
################################################################################
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# All build commands run from /src so relative project paths remain stable in
# local Docker builds, Railway builds and CI-style builders.
WORKDIR /src

# Copy the project file first to maximize Docker layer caching. Dependency
# restore is invalidated only when the .csproj changes, not on every source edit.
COPY Buildline/Platform/Buildline.Platform.csproj Buildline/Platform/

# Restore NuGet dependencies before copying the rest of the repository. This is
# the standard ASP.NET Core Docker pattern for faster iterative builds.
RUN dotnet restore Buildline/Platform/Buildline.Platform.csproj

# Copy the complete repository after restore so source code, migrations, JSON
# seed data, resources and XML documentation settings are available to publish.
COPY . .

# Publish a Release build into /app/publish. UseAppHost=false avoids generating
# a platform-specific native host executable and keeps the output portable inside
# the Linux runtime container.
RUN dotnet publish Buildline/Platform/Buildline.Platform.csproj \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

################################################################################
# Stage 2: Runtime image
# ------------------------------------------------------------------------------
# The ASP.NET runtime image contains only the framework/runtime dependencies
# needed to execute the already-published application.
################################################################################
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

# /app is the conventional working directory for published ASP.NET Core output.
WORKDIR /app

# Railway expects the containerized process to listen on the port exposed by the
# service. 8080 is used consistently in Dockerfile, Program.cs documentation and
# Railway variables. The + binding accepts traffic on all container interfaces.
ENV ASPNETCORE_URLS=http://+:8080

# Production is the default container environment. Override only for local
# diagnostics when Development-only behavior is intentionally needed.
ENV ASPNETCORE_ENVIRONMENT=Production

# Swagger is enabled by default in Production for Sprint Review evidence.
# Set ENABLE_SWAGGER=false in Railway later if the public Swagger UI must be hidden.
ENV ENABLE_SWAGGER=true

# Copy the published output from the build stage. No SDK files, obj folders or
# source-only build artifacts are copied into the final image.
COPY --from=build /app/publish .

# Document the internal port used by the app. Railway still controls public
# routing, but EXPOSE makes the image contract clear to humans and tooling.
EXPOSE 8080

# Start the ASP.NET Core application. Configuration is supplied through
# appsettings.*.json plus Railway environment variables expanded by Program.cs.
ENTRYPOINT ["dotnet", "Buildline.Platform.dll"]
