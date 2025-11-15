# ========================================================================
# Deploy Kubernetes - Plataforma Educacional (MBA DevXpert)
# ========================================================================

$k8sPath = "./k8s"
$namespace = "plataforma-educacional"

Write-Host ""
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host "===  Deploy Kubernetes - Plataforma Educacional (MBA DevXpert) ===" -ForegroundColor Cyan
Write-Host "===  Caminho dos Manifests: $k8sPath                              " -ForegroundColor Yellow
Write-Host "===  Namespace: $namespace                                        " -ForegroundColor Yellow
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host ""

# ------------------------------------------------------------------------
# 1) Verifica se o Minikube esta em execucao
# ------------------------------------------------------------------------
Write-Host "Verificando status do Minikube..." -ForegroundColor Yellow
$minikubeStatus = & minikube status | Select-String "host: Running"

if (-not $minikubeStatus) {
    Write-Host "Minikube nao esta em execucao. Iniciando cluster..." -ForegroundColor Yellow
    & minikube start --driver=docker
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Falha ao iniciar Minikube." -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "✅ Minikube ja esta ativo." -ForegroundColor Green
}

# ------------------------------------------------------------------------
# 2) Cria o namespace se nao existir
# ------------------------------------------------------------------------
Write-Host "Aplicando namespace..." -ForegroundColor Yellow
kubectl apply -f "$k8sPath/namespace.yml"
if ($LASTEXITCODE -ne 0) {
    Write-Host "Falha ao aplicar namespace." -ForegroundColor Red
    exit 1
}

# ------------------------------------------------------------------------
# 3) Aplica infraestrutura (SQL, RabbitMQ, Redis)
# ------------------------------------------------------------------------
Write-Host "Aplicando infraestrutura..." -ForegroundColor Yellow
kubectl apply -f "$k8sPath/infra/" -n $namespace
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Falha ao aplicar infraestrutura." -ForegroundColor Red
    exit 1
}

# ------------------------------------------------------------------------
# 4) Aguarda subida dos pods de infra
# ------------------------------------------------------------------------
Write-Host "Aguardando pods de infraestrutura ficarem prontos..." -ForegroundColor Yellow
kubectl wait --for=condition=Ready pods --all -n $namespace --timeout=180s

# ------------------------------------------------------------------------
# 5) Aplica microsservicos (APIs + BFF + Frontend)
# ------------------------------------------------------------------------
Write-Host "Aplicando microsservicos (APIs + BFF + Frontend)..." -ForegroundColor Yellow
kubectl apply -f "$k8sPath/services/" -n $namespace
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Falha ao aplicar servicos." -ForegroundColor Red
    exit 1
}

# ------------------------------------------------------------------------
# 6) Exibe status dos pods e services
# ------------------------------------------------------------------------
Write-Host ""
Write-Host "====================================================" -ForegroundColor Cyan
Write-Host "✅  Recursos implantados com sucesso!" -ForegroundColor Green
Write-Host "====================================================" -ForegroundColor Cyan
Write-Host ""

kubectl get pods -n $namespace
Write-Host ""
kubectl get svc -n $namespace

# ------------------------------------------------------------------------
# 7) (Opcional) Abre o frontend no navegador
# ------------------------------------------------------------------------
Write-Host ""
Write-Host "Abrindo o frontend via Minikube..." -ForegroundColor Yellow
minikube service frontend -n $namespace


