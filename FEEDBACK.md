# FEEDBACK – Avaliação Parcial DevOps

## Organização e Estrutura do Projeto

### Pontos Positivos

- **Estrutura de pastas bem organizada**: Separação clara entre `src/backend`, `src/frontend`, `data`, `docker`, `k8s`, `scripts` e `docs`, seguindo padrões profissionais de DevOps.
- **Arquivo de solução (.sln)** presente na raiz (`src/backend/MBA.Modulo5.sln`) com todos os projetos referenciados corretamente.
- **Dockerfiles bem estruturados** com multi-stage build, imagens base oficiais (`mcr.microsoft.com/dotnet:9.0`), curl instalado e variáveis de ambiente configuradas por serviço.
- **Manifestos Kubernetes completos** em `k8s/`: Namespace, Deployments, Services, ConfigMaps e Secrets para cada microsserviço com health checks.
- **Workflows GitHub Actions** com stages bem definidos: lint, build, testes e deploy em Docker Hub.
- **Scripts PowerShell de automação**: start-container.ps1, start-k8s.minikube.ps1 para provisionar ambiente.
- **Git com branching model**: main, DEV e features/* com Pull Requests para integração.
- **README.md completo**: Visão geral, arquitetura, instruções de execução.

### Pontos Negativos

- **Arquivo `.dockerignore` ausente**: Não há otimização de contexto. Deveria excluir `bin/`, `obj/`, `.git`, `TestResults`.
- **Arquivo `Rascunho.txt` desnecessário**: Deveria estar em .gitignore ou ser removido.
- **Arquivo `MBA.Modulo4.slnLaunch.user` versionado**: Não deveria estar no repositório.
- **Entrypoint script com potencial CRLF**: Arquivo `docker/infra/sqlserver-init/entrypoint.sh` pode ter line endings incorretos.
- **Healthcheck em docker-compose.auth-api.yml**: Porta incorreta (8080 vs 5001 da aplicação).

---

## Pipeline CI/CD

### Pontos Positivos

- **Workflows GitHub Actions funcionais**: Lint, build, testes em cada PR/push com cache habilitado.
- **Testes automatizados**: Unit e integração executados, artefatos salvos.
- **Testes completos na main**: Job `solution-tests` executa MBA.Modulo5.sln apenas em push → main.
- **Deploy Docker Hub automatizado**: Versionamento de imagens (latest, versão csproj, git sha).

### Pontos Negativos

- **Sem análise estática avançada**: Apenas `dotnet format`.
- **Sem SAST (Snyk, Dependabot)**: Sem verificação de vulnerabilidades em dependências.
- **Sem artefato de cobertura**: Cobertura coletada mas não publicada no workflow.

---

## Containerização

### Pontos Positivos

- **Multi-stage builds**: Padrão correto (base, build, final).
- **Layers otimizadas**: Restore antes de copiar código.
- **Curl instalado**: Para health checks.
- **Imagens em Docker Hub**: Acessíveis para orquestração.

### Pontos Negativos

- **Sem `.dockerignore`**: Contexto inteiro aumenta build.
- **Versão base não fixa**: `aspnet:9.0` puxa última minor. Deveria ser `aspnet:9.0.1`.

---

## Orquestração Kubernetes

### Pontos Positivos

- **Namespace centralizado**: `plataforma-educacional`.
- **Deployments profissionais**: Labels, seletores, image pull policy, environment variables.
- **Health checks**: Liveness e readiness probes com timeouts apropriados.
- **Resource management**: Requests e limits definidos.
- **Services e ConfigMaps**: Bem estruturados.

### Pontos Negativos

- **Replicas: 1**: Sem escalabilidade. Deveria ser ≥2.
- **Secrets em plaintext**: YAML no repositório expõe credenciais (Connection strings, JWT keys).
- **Sem Ingress**: Sem exposição externa.
- **Sem PersistentVolume**: Dados SQL perdem-se ao destruir pod.
- **Sem network policies**: Sem restrição de tráfego entre pods.

---

## Resiliência e Observabilidade

### Pontos Positivos

- **Retry policies com Polly**: MessageBus com exponential backoff (5 tentativas).
- **Circuit Breaker**: BFF-API configura para HttpClient.
- **Health checks**: Endpoint `/health` em cada API, liveness/readiness no K8s.
- **Restart policies**: `unless-stopped` em docker-compose.

### Pontos Negativos

- **Health checks genéricos**: Retornam status hardcoded. Deveria validar banco/RabbitMQ/Redis.
- **Sem logs centralizados**: Sem ELK Stack ou Splunk.
- **Sem métricas**: Sem Prometheus/Grafana.
- **Sem distributed tracing**: Sem Jaeger/OpenTelemetry.

---

## Qualidade do Código

### Pontos Positivos

- **522+ testes passam**: Execução bem-sucedida.
- **Method coverage: 94.1%** (excelente).
- **Testes bem estruturados**: Mocks, Arrange-Act-Assert, Fixtures.
- **DDD implementado**: Bounded Contexts, agregados, value objects.
- **CQRS**: Commands, Queries, Handlers separados.

### Pontos Negativos

- **Branch coverage: 79%**: 1 ponto abaixo do mínimo (80%). Edge cases não testados.
- **Program.cs excluído**: Setup não testado.
- **Testes integração superficiais**: `TesteBasico_DevePassar()` é apenas `true.Should().BeTrue()`.
- **Sem testes de segurança**: Endpoints JWT não validados.
- **Código duplicado**: `// GIT Force rebuild` repetido 6x em Program.cs.

---

## Segurança

### Pontos Positivos

- **JWT implementado**: JwtSettings, roles (Admin/Usuario).
- **Secrets não em appsettings.json**.
- **Imagens base oficiais**.

### Pontos Negativos

- **Credentials expostos**: K8s Secrets e docker-compose contêm senhas em plaintext.
- **Senhas em YAML**: `Password=PlataformaEducacional123!` visível no repositório.
- **Sem validação de entrada robusta**.
- **Sem criptografia**: Dados de cartão armazenados sem proteção.
- **CORS AllowAll**: Inadequado para produção.

---

## Documentação

### Pontos Positivos

- **README.md completo**: Visão geral, arquitetura, instruções.
- **Swagger/OpenAPI**: Endpoints documentados com autenticação.
- **Scripts comentados**.

### Pontos Negativos

- **Falta contrato de APIs**: Sem documento de eventos/mensagens entre BCs.
- **Sem diagrama de arquitetura**: Ajudaria visual de dependências.
- **README não descreve testes**: Falta `dotnet test` e geração de cobertura.
- **Sem troubleshooting**: Como debugar falhas.
- **K8s docs incompleta**: Passo a passo do Minikube faltando.

---

## Conclusão

Projeto **DevOps bem estruturado** com pipeline, Docker e Kubernetes. Method coverage 94.1%, 522+ testes passando.

✅ **Fortes**: CI/CD funcional, multi-stage builds, health checks K8s, Polly resilience, automation.

⚠️ **Áreas**: Branch coverage 79% (< 80%), secrets expostos, `.dockerignore` faltando, health checks genéricos, sem ingress/PV, docs incompleta.

