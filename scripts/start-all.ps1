Clear

$dockerFilePath = "./docker"
$networkName = "plataforma-network"

Write-Host ""
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "=== Iniciando ambiente Plataforma Educacional - MBA DevXpert    " -ForegroundColor Cyan
Write-Host "=== Path de Trabalho: $dockerFilePath                           " -ForegroundColor Yellow
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""

## Remover imagens intermediárias (dangling images)
#Write-Host "Removendo imagens intermediarias..." -ForegroundColor Red
#docker image prune -f | Out-Null
#docker builder prune -f | Out-Null

# ------------------------------------------------------------------------
# 1) Garante que a rede exista antes de subir qualquer container
# ------------------------------------------------------------------------
Write-Host "Verificando rede '$networkName'..." -ForegroundColor Yellow
docker network inspect $networkName 2>$null

if ($LASTEXITCODE -eq 0) {
    Write-Host "Rede '$networkName' ja existe. Mantendo." -ForegroundColor Green
}
else {
    Write-Host "Rede '$networkName' nao encontrada. Criando..." -ForegroundColor Yellow
    docker network create $networkName -d bridge | Out-Null
}

Write-Host "Aguardando 5 segundos para 'warmup' de Network." -ForegroundColor Red
docker network ls
Start-Sleep -Seconds 5

# ------------------------------------------------------------------------
# 2) Sobe infraestrutura base
# ------------------------------------------------------------------------
Write-Host "Subindo infraestrutura (SQL Server, RabbitMQ, Redis)..." -ForegroundColor Yellow
$infra = @( @{ Name = "Sql Server"; Compose = "$dockerFilePath/infra/docker-compose.sqlserver.yml" },
            @{ Name = "RabbitMQ";   Compose = "$dockerFilePath/infra/docker-compose.rabbitmq.yml" },
            @{ Name = "Redis";      Compose = "$dockerFilePath/infra/docker-compose.redis.yml" }
)
foreach ($inf in $infra) {
  Write-Host "Subindo imagem $($inf.Name)..." -ForegroundColor Cyan
  docker compose -f $inf.Compose up -d 
  
  if ($LASTEXITCODE -ne 0) {
    Write-Host "Falha ao subir imagem $($inf.Tag)." -ForegroundColor Red
    exit 1
  }
}

Write-Host "Aguardando 5 segundos para 'warmup' de Infra." -ForegroundColor Red
Start-Sleep -Seconds 5

#------------------------------------------------------------------------
#3) Garante que as imagens das APIs e do frontend existam. É realizado o Build e Compose de cada API e Front
#------------------------------------------------------------------------
Write-Host "Verificando e construindo imagens (se necessario)..." -ForegroundColor Yellow
$services = @( @{ Name = "conteudo-api";   Path = "./src/backend/conteudo-api/Dockerfile";   Tag = "educa/conteudo-api:latest";   Context = "./src/backend";  Compose = "$dockerFilePath/services/docker-compose.conteudo-api.yml" },
               @{ Name = "pagamentos-api"; Path = "./src/backend/pagamentos-api/Dockerfile"; Tag = "educa/pagamentos-api:latest"; Context = "./src/backend";  Compose = "$dockerFilePath/services/docker-compose.pagamentos-api.yml" },
               @{ Name = "alunos-api";     Path = "./src/backend/alunos-api/Dockerfile";     Tag = "educa/alunos-api:latest";     Context = "./src/backend";  Compose = "$dockerFilePath/services/docker-compose.alunos-api.yml" },
               @{ Name = "auth-api";       Path = "./src/backend/auth-api/Dockerfile";       Tag = "educa/auth-api:latest";       Context = "./src/backend";  Compose = "$dockerFilePath/services/docker-compose.auth-api.yml" },
               @{ Name = "bff-api";        Path = "./src/backend/bff-api/Dockerfile";        Tag = "educa/bff-api:latest";        Context = "./src/backend";  Compose = "$dockerFilePath/services/docker-compose.bff-api.yml" },
               @{ Name = "frontend";       Path = "./src/frontend/Dockerfile";               Tag = "educa/frontend:latest";       Context = "./src/frontend"; Compose = "$dockerFilePath/services/docker-compose.frontend.yml" }
)

foreach ($svc in $services) {
  Write-Host "Construindo imagem para $($svc.Name)..." -ForegroundColor Cyan
  docker build -t $svc.Tag -f $svc.Path $svc.Context
  
  if ($LASTEXITCODE -ne 0) {
    Write-Host "Falha ao construir imagem $($svc.Tag)." -ForegroundColor Red
    exit 1
  }
  
  Write-Host "Subindo container para $($svc.Name)..." -ForegroundColor Cyan
  docker compose -f $svc.Compose up -d 
  
  if ($LASTEXITCODE -ne 0) {
    Write-Host "Falha ao subir container $($svc.Tag)." -ForegroundColor Red
    exit 1
  }
}

Write-Host "Aguardando 5 segundos após 'Docker Compose'." -ForegroundColor Red
Start-Sleep -Seconds 5

if ($LASTEXITCODE -ne 0) {
  Write-Host "Erro ao subir microsservicos." -ForegroundColor Red
  exit 1
}

# ------------------------------------------------------------------------
# 4) Exibe status final
# ------------------------------------------------------------------------
Write-Host ""
Write-Host "====================================================" -ForegroundColor Green
Write-Host "=== Ambiente iniciado com sucesso!                  " -ForegroundColor Green
Write-Host "====================================================" -ForegroundColor Green
Write-Host ""

docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
