# Base image per runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

# Build image (usare nanoserver per coerenza)
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia solo il .csproj inizialmente
COPY ["./Metoda Reporting Web App.csproj", "MvcClient/"]
RUN dotnet restore "MvcClient/Metoda Reporting Web App.csproj"

# Copia tutto il progetto
COPY . .
COPY ../AuthServer/cert.pfx .

RUN certca.exe -importpfx My cert.pfx password-certificato

WORKDIR "/src/MvcClient"

RUN dotnet publish "./Metoda Reporting Web App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MetodaReportingWebApp.dll"]
