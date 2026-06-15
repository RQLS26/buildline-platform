FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Buildline/Platform/Buildline.Platform.csproj Buildline/Platform/
RUN dotnet restore Buildline/Platform/Buildline.Platform.csproj

COPY . .
RUN dotnet publish Buildline/Platform/Buildline.Platform.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Buildline.Platform.dll"]

