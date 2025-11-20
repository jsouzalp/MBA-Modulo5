# ========================================================================
# Etapa 1 - Build
# ========================================================================
# Define a imagem a ser utilizada na API
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5004

# Define a imagem a ser utilizada no build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar solução
COPY ["pagamentos-api/Pagamentos.API.sln", "pagamentos-api/"]

# Copiar projetos referenciados (incluindo os building-blocks)
COPY ["pagamentos-api/src/Pagamentos.API/Pagamentos.API.csproj", "pagamentos-api/src/Pagamentos.API/"]
COPY ["pagamentos-api/src/Pagamentos.Application/Pagamentos.Application.csproj", "pagamentos-api/src/Pagamentos.Application/"]
COPY ["pagamentos-api/src/Pagamentos.Domain/Pagamentos.Domain.csproj", "pagamentos-api/src/Pagamentos.Domain/"]
COPY ["pagamentos-api/src/Pagamentos.Infrastructure/Pagamentos.Infrastructure.csproj", "pagamentos-api/src/Pagamentos.Infrastructure/"]
COPY ["pagamentos-api/src/Pagamento.AntiCorruption/Pagamento.AntiCorruption.csproj", "pagamentos-api/src/Pagamento.AntiCorruption/"]
COPY ["building-blocks/core/Core.csproj", "building-blocks/core/"]
COPY ["building-blocks/MessageBus/MessageBus.csproj", "building-blocks/MessageBus/"]

# ========================================================================
# Etapa 2 - Restore de dependências
# ========================================================================
RUN dotnet restore "pagamentos-api/Pagamentos.API.sln"

# Copiar todo o restante do código
COPY . .

# Compilar e publicar
RUN dotnet build "pagamentos-api/src/Pagamentos.API/Pagamentos.API.csproj" -c Release

RUN dotnet publish "pagamentos-api/src/Pagamentos.API/Pagamentos.API.csproj" -c Release -o /out /p:UseAppHost=false

# ========================================================================
# Etapa 3 - Runtime
# ========================================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /out .
ENV ASPNETCORE_URLS=http://+:5004

EXPOSE 5004
ENTRYPOINT ["dotnet", "Pagamentos.API.dll"]
