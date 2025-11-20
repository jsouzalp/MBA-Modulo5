# ========================================================================
# Stop Docker Environment - Plataforma Educacional (MBA DevXpert)
# ========================================================================

$dockerFilePath = "./docker"
$networkName = "plataforma-network"

Write-Host ""
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "=== Parando ambiente Plataforma Educacional - MBA DevXpert  ==="  -ForegroundColor Cyan
Write-Host "=== Path de Trabalho: $dockerFilePath"                            -ForegroundColor Yellow
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""

# ------------------------------------------------------------------------
# 1) Verifica se a rede existe
# ------------------------------------------------------------------------
Write-Host "Verificando rede '$networkName'..." -ForegroundColor Yellow
$networkExists = docker network ls --format "{{.Name}}" | Select-String -SimpleMatch $networkName

if (-not $networkExists) {
    Write-Host "Rede '$networkName' nao encontrada. Pode ser que o ambiente ja esteja parado." -ForegroundColor Yellow
}

# ------------------------------------------------------------------------
# 2) Para microsserviços e frontend
# ------------------------------------------------------------------------
Write-Host "Verificando e derrubando containers..." -ForegroundColor Yellow
$services = @( @{ Name = "frontend";       Compose = "$dockerFilePath/services/docker-compose.frontend.yml"},
               @{ Name = "bff-api";        Compose = "$dockerFilePath/services/docker-compose.bff-api.yml" },
               @{ Name = "auth-api";       Compose = "$dockerFilePath/services/docker-compose.auth-api.yml" },
               @{ Name = "alunos-api";     Compose = "$dockerFilePath/services/docker-compose.alunos-api.yml" },
               @{ Name = "pagamentos-api"; Compose = "$dockerFilePath/services/docker-compose.pagamentos-api.yml" },
               @{ Name = "conteudo-api";   Compose = "$dockerFilePath/services/docker-compose.conteudo-api.yml" }
)

foreach ($svc in $services) {
  Write-Host "Parando container $($svc.Name)..." -ForegroundColor Cyan
  docker compose -f $svc.Compose down

  if ($LASTEXITCODE -ne 0) {
      Write-Host "Erro ao parar container $($svc.Name). Verifique os logs." -ForegroundColor Red
  }
}

# ------------------------------------------------------------------------
# 3) Para infraestrutura (SQL Server, RabbitMQ, Redis)
# ------------------------------------------------------------------------
Write-Host "Verificando e derrubando containers de infra-estrutura (SQL Server, RabbitMQ, Redis)..." -ForegroundColor Yellow
$infra = @( @{ Name = "Sql Server"; Compose = "$dockerFilePath/infra/docker-compose.sqlserver.yml" },
            @{ Name = "RabbitMQ";   Compose = "$dockerFilePath/infra/docker-compose.rabbitmq.yml"  },
            @{ Name = "Redis";      Compose = "$dockerFilePath/infra/docker-compose.redis.yml"     }
)
foreach ($inf in $infra) {
  Write-Host "Parando container $($inf.Name)..." -ForegroundColor Cyan
  docker compose -f $svc.Compose down

  if ($LASTEXITCODE -ne 0) {
      Write-Host "Erro ao parar container $($svc.Name). Verifique os logs." -ForegroundColor Red
  }
}

# ------------------------------------------------------------------------
# 5) Pergunta se deseja remover volumes
# ------------------------------------------------------------------------
$removeChoice = Read-Host "Deseja remover volumes? (s/n)"
if ($removeChoice -eq 's') {
    Write-Host "Removendo volumes e rede..." -ForegroundColor Yellow
    docker volume prune -f | Out-Null
} else {
    Write-Host "Volumes mantidos." -ForegroundColor Cyan
}

# ------------------------------------------------------------------------
# 5) Pergunta se deseja remover rede
# ------------------------------------------------------------------------
$removeChoice = Read-Host "Deseja remover rede '$networkName'? (s/n)"
if ($removeChoice -eq 's') {
    Write-Host "Removendo rede..." -ForegroundColor Yellow
    docker network rm $networkName | Out-Null
} else {
    Write-Host "Rede mantida." -ForegroundColor Cyan
}

# ------------------------------------------------------------------------
# 5️⃣  Resumo final
# ------------------------------------------------------------------------
Write-Host ""
Write-Host "====================================================" -ForegroundColor Green
Write-Host "=== Ambiente Docker parado com sucesso!"              -ForegroundColor Green
Write-Host "====================================================" -ForegroundColor Green
Write-Host ""

docker ps --format "table {{.Names}}\t{{.Status}}" | findstr /V "NAMES"
