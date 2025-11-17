# ========================================================================
# Destroy Kubernetes - Plataforma Educacional (MBA DevXpert)
# ========================================================================

$k8sPath = "./k8s"
$namespace = "plataforma-educacional"
$ErrorActionPreference = "Stop"

Write-Host ""
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "===  Destruindo ambiente Kubernetes - Plataforma Educacional ===" -ForegroundColor Cyan
Write-Host "===  Caminho dos Manifests: $k8sPath                         ===" -ForegroundColor Yellow
Write-Host "===  Namespace: $namespace                                   ===" -ForegroundColor Yellow
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""


# -----------------------------
# 1) Remover recursos por pasta
# -----------------------------
function Remove-Folder($path, $label) {
    Write-Host "`n[$label] Removendo '$path'..." -ForegroundColor Red
    kubectl delete -f $path -n $namespace --ignore-not-found
}

Remove-Folder "$k8sPath/services/frontend"       "1/6"
Remove-Folder "$k8sPath/services/bff-api"        "2/6"
Remove-Folder "$k8sPath/services/pagamentos-api" "3/6"
Remove-Folder "$k8sPath/services/alunos-api"     "4/6"
Remove-Folder "$k8sPath/services/conteudo-api"   "5/6"
Remove-Folder "$k8sPath/services/auth-api"       "6/6"

Remove-Folder "$k8sPath/infra/rabbitmq"          "1/3"
Remove-Folder "$k8sPath/infra/redis"             "2/3"
Remove-Folder "$k8sPath/infra/sqlserver"         "3/3"

# -----------------------------
# 2) Remover o namespace
# -----------------------------
Write-Host "`n[3/6] Removendo namespace '$namespace'..." -ForegroundColor Red
kubectl delete namespace $namespace --ignore-not-found

Write-Host "Aguardando namespace remover completamente..." -ForegroundColor Yellow
kubectl wait --for=delete namespace/$namespace --timeout=120s

# -----------------------------
# Estado final
# -----------------------------
Write-Host "`n[6/6] Namespaces restantes:" -ForegroundColor Cyan
kubectl get ns

Write-Host "====================================" -ForegroundColor Green
Write-Host "=== REMOCAO FINALIZADA COM SUCESSO! " -ForegroundColor Green
Write-Host "====================================" -ForegroundColor Green
