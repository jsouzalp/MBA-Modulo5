# ========================================================================
# Destroy Kubernetes - Plataforma Educacional (MBA DevXpert)
# ========================================================================

$k8sPath = "./k8s"
$namespace = "plataforma-educacional"

Write-Host ""
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "===  Destruindo ambiente Kubernetes - Plataforma Educacional ===" -ForegroundColor Cyan
Write-Host "===  Caminho dos Manifests: $k8sPath                         ===" -ForegroundColor Yellow
Write-Host "===  Namespace: $namespace                                   ===" -ForegroundColor Yellow
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""

# ------------------------------------------------------------------------
# 1️⃣  Confirmação opcional de segurança
# ------------------------------------------------------------------------
$confirmation = Read-Host "Tem certeza que deseja REMOVER todo o ambiente do namespace '$namespace'? (s/n)"
if ($confirmation -ne 's') {
    Write-Host "Operação cancelada pelo usuário." -ForegroundColor Yellow
    exit 0
}

# ------------------------------------------------------------------------
# 2️⃣  Verifica se Minikube está ativo
# ------------------------------------------------------------------------
Write-Host "Verificando status do Minikube..." -ForegroundColor Yellow
$minikubeStatus = & minikube status | Select-String "host: Running"

if (-not $minikubeStatus) {
    Write-Host "⚠️  Minikube não está ativo. Nada a destruir." -ForegroundColor Red
    exit 0
}

# ------------------------------------------------------------------------
# 3️⃣  Remove os recursos do namespace
# ------------------------------------------------------------------------
Write-Host "Removendo recursos do namespace '$namespace'..." -ForegroundColor Yellow
kubectl delete all --all -n $namespace
kubectl delete configmap --all -n $namespace
kubectl delete secret --all -n $namespace
kubectl delete pvc --all -n $namespace

# ------------------------------------------------------------------------
# 4️⃣  Remove o namespace (limpa tudo)
# ------------------------------------------------------------------------
Write-Host "Removendo namespace..." -ForegroundColor Yellow
kubectl delete namespace $namespace --ignore-not-found=true

# ------------------------------------------------------------------------
# 5️⃣  Verifica se ainda há pods residuais
# ------------------------------------------------------------------------
Write-Host "Verificando pods residuais..." -ForegroundColor Yellow
$pods = kubectl get pods -A | Select-String $namespace
if ($pods) {
    Write-Host "⚠️  Alguns pods ainda estão sendo encerrados. Aguarde alguns segundos..." -ForegroundColor Yellow
    Start-Sleep -Seconds 10
}

# ------------------------------------------------------------------------
# 6️⃣  Pergunta se deseja parar o Minikube
# ------------------------------------------------------------------------
$stopChoice = Read-Host "Deseja parar o Minikube também? (s/n)"
if ($stopChoice -eq 's') {
    Write-Host "Parando o Minikube..." -ForegroundColor Yellow
    & minikube stop
}

# ------------------------------------------------------------------------
# Limpa qualquer resquício de objetos no host
# ------------------------------------------------------------------------
kubectl get ns
minikube delete --purge

# ------------------------------------------------------------------------
# 7️⃣  Finalização
# ------------------------------------------------------------------------
Write-Host ""
Write-Host "====================================================" -ForegroundColor Green
Write-Host "✅  Ambiente Kubernetes removido com sucesso!      " -ForegroundColor Green
Write-Host "====================================================" -ForegroundColor Green
Write-Host ""
