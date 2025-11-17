# ========================================================================
# Deploy Kubernetes - Plataforma Educacional (MBA DevXpert)
# ========================================================================

$k8sPath = "./k8s"
$namespace = "plataforma-educacional"
$ErrorActionPreference = "Stop"

Write-Host ""
Write-Host "===================================================================" -ForegroundColor Cyan
Write-Host "===  Inicializando ambiente Kubernetes - Plataforma Educacional    " -ForegroundColor Cyan
Write-Host "===  Caminho dos Manifests: $k8sPath                               " -ForegroundColor Yellow
Write-Host "===  Namespace: $namespace                                         " -ForegroundColor Yellow
Write-Host "===================================================================" -ForegroundColor Cyan
Write-Host ""

# Criar namespace somente se não existir
Write-Host "`n[1/10] Verificando namespace..." -ForegroundColor Green
$nsExists = kubectl get namespace $namespace --ignore-not-found

if (-not $nsExists) {
    Write-Host "Namespace '$namespace' nao existe. Criando..." -ForegroundColor Yellow
    kubectl create namespace $namespace
} else {
    Write-Host "Namespace '$namespace' ja existe." -ForegroundColor DarkGreen
}

# -----------------------------
# Função para aplicar pasta
# -----------------------------
function Apply-Folder($path, $label) {
    Write-Host "`n[$label] Aplicando '$path'..." -ForegroundColor Green
    kubectl apply -f $path -n $namespace
}

# -----------------------------
# 2) Infraestrutura
# -----------------------------
Apply-Folder "$k8sPath/infra/sqlserver"    "2/10"
Apply-Folder "$k8sPath/infra/redis"        "3/10"
Apply-Folder "$k8sPath/infra/rabbitmq"     "4/10"

# -----------------------------
# 3) Aguardar SQL Server
# -----------------------------
Write-Host "`n[5/7] Aguardando SQL Server ficar pronto..." -ForegroundColor Green
kubectl wait --for=condition=ready pod -l app=sqlserver -n $namespace --timeout=180s

# -----------------------------
# 4) Serviços
# -----------------------------
Apply-Folder "$k8sPath/services/auth-api"        "5/10"
Apply-Folder "$k8sPath/services/conteudo-api"    "6/10"
Apply-Folder "$k8sPath/services/alunos-api"      "7/10"
Apply-Folder "$k8sPath/services/pagamentos-api"  "8/10"
Apply-Folder "$k8sPath/services/bff-api"         "9/10"
Apply-Folder "$k8sPath/services/frontend"        "10/10"

# -----------------------------
# 5) Mostrar estado final
# -----------------------------
Write-Host "`n[7/7] Estado final dos pods:" -ForegroundColor Cyan
kubectl get pods -n $namespace

Write-Host "`nServiços:" -ForegroundColor Cyan
kubectl get svc -n $namespace

Write-Host "====================================" -ForegroundColor Green
Write-Host "=== DEPLOY FINALIZADO COM SUCESSO!  " -ForegroundColor Green
Write-Host "====================================" -ForegroundColor Green
