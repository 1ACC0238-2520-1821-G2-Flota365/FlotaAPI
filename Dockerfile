# ====== STAGE 1: Build ======
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivo de proyecto y restaurar dependencias
COPY BackendWeb/BackendWeb.csproj ./BackendWeb/
RUN dotnet restore ./BackendWeb/BackendWeb.csproj

# Copiar el resto del código y compilar
COPY BackendWeb ./BackendWeb
WORKDIR /src/BackendWeb
RUN dotnet publish -c Release -o /app/publish

# ====== STAGE 2: Runtime ======
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app

ENV DOTNET_EnableDiagnostics=0 \
    ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BackendWeb.dll"]
