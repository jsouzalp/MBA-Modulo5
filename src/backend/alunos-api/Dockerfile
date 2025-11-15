# ========================================================================
# Etapa 1 - Build
# ========================================================================
# Define a imagem a ser utilizada na API
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5003

# Define a imagem a ser utilizada no build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar solução
COPY ["alunos-api/Alunos.API.sln", "alunos-api/"]

# Copiar projetos referenciados (incluindo os building-blocks)
COPY ["alunos-api/src/Alunos.API/Alunos.API.csproj", "alunos-api/src/Alunos.API/"]
COPY ["alunos-api/src/Alunos.Application/Alunos.Application.csproj", "alunos-api/src/Alunos.Application/"]
COPY ["alunos-api/src/Alunos.Domain/Alunos.Domain.csproj", "alunos-api/src/Alunos.Domain/"]
COPY ["alunos-api/src/Alunos.Infrastructure/Alunos.Infrastructure.csproj", "alunos-api/src/Alunos.Infrastructure/"]
COPY ["building-blocks/core/Core.csproj", "building-blocks/core/"]
COPY ["building-blocks/MessageBus/MessageBus.csproj", "building-blocks/MessageBus/"]

# ========================================================================
# Etapa 2 - Restore de dependências
# ========================================================================
RUN dotnet restore "alunos-api/Alunos.API.sln"

# Copiar todo o restante do código
COPY . .

# Compilar e publicar
RUN dotnet build "alunos-api/src/Alunos.API/Alunos.API.csproj" -c Release

RUN dotnet publish "alunos-api/src/Alunos.API/Alunos.API.csproj" -c Release -o /out /p:UseAppHost=false

# ========================================================================
# Etapa 3 - Runtime
# ========================================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /out .

ENV ASPNETCORE_URLS=http://+:5003
EXPOSE 5003
ENTRYPOINT ["dotnet", "Alunos.API.dll"]
