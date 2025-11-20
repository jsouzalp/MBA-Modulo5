# ========================================================================
# Etapa 1 - Build
# ========================================================================
# Define a imagem a ser utilizada na API
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5002

# Define a imagem a ser utilizada no build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar solução
COPY ["conteudo-api/Conteudo.API.sln", "conteudo-api/"]

# Copiar projetos referenciados (incluindo os building-blocks)
COPY ["conteudo-api/src/Conteudo.API/Conteudo.API.csproj", "conteudo-api/src/Conteudo.API/"]
COPY ["conteudo-api/src/Conteudo.Application/Conteudo.Application.csproj", "conteudo-api/src/Conteudo.Application/"]
COPY ["conteudo-api/src/Conteudo.Domain/Conteudo.Domain.csproj", "conteudo-api/src/Conteudo.Domain/"]
COPY ["conteudo-api/src/Conteudo.Infrastructure/Conteudo.Infrastructure.csproj", "conteudo-api/src/Conteudo.Infrastructure/"]
COPY ["building-blocks/core/Core.csproj", "building-blocks/core/"]
COPY ["building-blocks/MessageBus/MessageBus.csproj", "building-blocks/MessageBus/"]

# ========================================================================
# Etapa 2 - Restore de dependências
# ========================================================================
RUN dotnet restore "conteudo-api/Conteudo.API.sln"

# Copiar todo o restante do código
COPY . .

# Compilar e publicar
RUN dotnet build "conteudo-api/src/Conteudo.API/Conteudo.API.csproj" -c Release

RUN dotnet publish "conteudo-api/src/Conteudo.API/Conteudo.API.csproj" -c Release -o /out /p:UseAppHost=false

# ========================================================================
# Etapa 3 - Runtime
# ========================================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /out .

ENV ASPNETCORE_URLS=http://+:5002
EXPOSE 5002
ENTRYPOINT ["dotnet", "Conteudo.API.dll"]