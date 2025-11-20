# Usar a imagem base do .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5005

# Usar a imagem SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto
COPY ["src/BFF.API/BFF.API.csproj", "src/BFF.API/"]
COPY ["src/BFF.Application/BFF.Application.csproj", "src/BFF.Application/"]
COPY ["src/BFF.Domain/BFF.Domain.csproj", "src/BFF.Domain/"]
COPY ["src/BFF.Infrastructure/BFF.Infrastructure.csproj", "src/BFF.Infrastructure/"]

# Restaurar dependências
RUN dotnet restore "src/BFF.API/BFF.API.csproj"

# Copiar todo o código fonte
COPY . .

# Build da aplicação
WORKDIR "/src/src/BFF.API"
RUN dotnet build "BFF.API.csproj" -c Release -o /app/build

# Publish da aplicação
FROM build AS publish
RUN dotnet publish "BFF.API.csproj" -c Release -o /app/publish

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configurar variáveis de ambiente
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5005

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "BFF.API.dll"] 