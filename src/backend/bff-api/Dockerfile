# ========================================================================
# Etapa 1 - Build
# ========================================================================
# Define a imagem a ser utilizada na API
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

# Define a imagem a ser utilizada no build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar solução
COPY ["bff-api/BFF.API.sln", "bff-api/"]

# Copiar projetos referenciados (incluindo os building-blocks)
COPY ["bff-api/src/BFF.API/BFF.API.csproj", "bff-api/src/BFF.API/"]
COPY ["bff-api/src/BFF.Application/BFF.Application.csproj", "bff-api/src/BFF.Application/"]
COPY ["bff-api/src/BFF.Domain/BFF.Domain.csproj", "bff-api/src/BFF.Domain/"]
COPY ["bff-api/src/BFF.Infrastructure/BFF.Infrastructure.csproj", "bff-api/src/BFF.Infrastructure/"]
COPY ["building-blocks/core/Core.csproj", "building-blocks/core/"]
COPY ["building-blocks/MessageBus/MessageBus.csproj", "building-blocks/MessageBus/"]

# ========================================================================
# Etapa 2 - Restore de dependências
# ========================================================================
RUN dotnet restore "bff-api/BFF.API.sln"

# Copiar todo o restante do código
COPY . .

# Compilar e publicar
RUN dotnet build "bff-api/src/BFF.API/BFF.API.csproj" -c Release

RUN dotnet publish "bff-api/src/BFF.API/BFF.API.csproj" -c Release -o /out /p:UseAppHost=false

# ========================================================================
# Etapa 3 - Runtime
# ========================================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /out .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "BFF.API.dll"]
