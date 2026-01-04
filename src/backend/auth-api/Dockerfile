# ========================================================================
# Etapa 1 - Build
# ========================================================================
# Define a imagem a ser utilizada na API
FROM mcr.microsoft.com/dotnet/aspnet:9.0.1 AS base
WORKDIR /app
EXPOSE 5001

# Define a imagem a ser utilizada no build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar solução
COPY ["auth-api/Auth.API.sln", "auth-api/"]

# Copiar projetos referenciados (incluindo os building-blocks)
COPY ["auth-api/src/Auth.API/Auth.API.csproj", "auth-api/src/Auth.API/"]
COPY ["auth-api/src/Auth.Application/Auth.Application.csproj", "auth-api/src/Auth.Application/"]
COPY ["auth-api/src/Auth.Domain/Auth.Domain.csproj", "auth-api/src/Auth.Domain/"]
COPY ["auth-api/src/Auth.Infrastructure/Auth.Infrastructure.csproj", "auth-api/src/Auth.Infrastructure/"]
COPY ["building-blocks/core/Core.csproj", "building-blocks/core/"]
COPY ["building-blocks/MessageBus/MessageBus.csproj", "building-blocks/MessageBus/"]

# ========================================================================
# Etapa 2 - Restore de dependências
# ========================================================================
RUN dotnet restore "auth-api/Auth.API.sln"

# Copiar todo o restante do código
COPY . .

# Compilar e publicar
RUN dotnet build "auth-api/src/Auth.API/Auth.API.csproj" -c Release

RUN dotnet publish "auth-api/src/Auth.API/Auth.API.csproj" -c Release -o /out /p:UseAppHost=false

# ========================================================================
# Etapa 3 - Runtime
# ========================================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0.1 AS final
WORKDIR /app

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /out .

ENV ASPNETCORE_URLS=http://+:5001
EXPOSE 5001
ENTRYPOINT ["dotnet", "Auth.API.dll"]
