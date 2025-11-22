# 📘 Plataforma Educacional — Projeto 5 (MBA DevXpert)

> Arquitetura de Microsserviços • Docker • Docker Hub • CI/CD • GitHub Actions • Testes automatizados

## 📑 Sumário

1. 🧩 Visão Geral
2. 🏗 Arquitetura de Microsserviços
3. 🔄 Github Actions (pipelines)
4. 🗄️ Base de Dados
5. 🐳 Contêineres e Docker Compose
6. ☸️ Minikube
7. 📜 Scripts PowerShell
8. 📂 Estrutura do Código Fonte
9. ▶️ Executando localmente
10. 🌐 Executando via Docker Hub
11. 🧪 Testes Automatizados
12. 🔄 Branch Strategy
13. 🤖 CI/CD
14. 🏷 Badges
15. 🐳 Imagens no Docker Hub
16. 🧰 Tecnologias
17. 📜 Licença

## 🧩 Visão Geral

Este projeto implementa uma plataforma educacional distribuída, composta por diversos microsserviços independentes, cada um responsável por um domínio funcional específico.

O foco do Projeto 5 é **infraestrutura e DevOps**, incluindo:

- Containers Docker para todos os microsserviços  
- Build e versionamento automatizado  
- Publicação das imagens no Docker Hub  
- Pipelines CI/CD independentes por microsserviço  
- Testes automatizados  
- Padronização de branches (DEV → PR → main)  
- Execução local via Docker Compose  
- Scripts PowerShell de automação  

## 🏗 Arquitetura de Microsserviços

A solução é composta pelos microsserviços:

### 🌐 BFF API  
Backend for Frontend: camada intermediária entre o frontend e os microsserviços internos.

### 🔐 Auth API  
API responsável por autenticação, geração e validação de tokens JWT, login e controle de identidade.

### 📚 Conteúdo API  
Gerencia cursos, aulas, metadados e materiais educacionais.

### 🎓 Alunos API  
Gerencia alunos, matrículas, certificados e progresso acadêmico.

### 💰 Pagamentos API  
Responsável por operações de pagamento e transações.

## 🔄 Github Actions (pipelines)
```
.github/
 └── workflows
       ├── alunos-api.yml
       ├── auth-api.yml
       ├── bff-api.yml
       ├── conteudo-api.yml
       ├── frontend.yml
       └── pagamentos-api.yml
```

## 🗄️ Estrutura de base de dados
```
data/
 ├── 0000_UP_CreateDatabase_Aluno_API.sql
 ├── 0000_UP_CreateDatabase_Auth_API.sql
 ├── 0000_UP_CreateDatabase_Conteudo_API.sql
 ├── 0000_UP_CreateDatabase_Pagamento_API.sql
 ├── 0001_UP_Aluno.Api.sql
 ├── 0001_UP_Auth.Api.sql
 ├── 0001_UP_Conteudo.Api.sql
 └── 0001_UP_Pagamento.Api.sql
```

## 🐳 Contêineres e Docker Compose
⚠️ IMPORTANTE:
O arquivo `docker/infra/sqlserver-init/entrypoint.sh` deve usar EOL no formato `LF`.
Se estiver em `CRLF`, o container do SQL Server NÃO iniciará e todo o ecossistema falhará.

```
docker/
  ├── infra/
  │     ├── sqlserver-init/
  │     │     └── entrypoint.sh
  │     ├── docker-compose.rabbitmq.yml
  │     ├── docker-compose.redis.yml
  │     └── docker-compose.sqlserver.yml
  ├── services/
  │     ├── docker-compose.alunos-api.yml
  │     ├── docker-compose.auth-api.yml
  │     ├── docker-compose.bff-api.yml
  │     ├── docker-compose.conteudo-api.yml
  │     ├── docker-compose.frontend.yml
  │     └── docker-compose.pagamentos-api.yml
  └── docker-compose.full.yml
```

## ☸️ Minikube *[Em andamento]*
A proposta é ter um ambiente Kubernetes local na máquina do desenvolvedor
```
k8s/
 ├── infra/
 │     ├── rabbitmq/
 │     │     ├── configmap.yml
 │     │     ├── deployment.yml
 │     │     └── service.yml
 │     ├── redis/
 │     │     ├── deployment.yml
 │     │     └── service.yml
 │     └── sqlserver/
 │           ├── deployment.yml
 │           ├── secret.yml
 │           └── service.yml
 ├── services/
 │     ├── alunos-api/
 │     │     ├── configmap.yml
 │     │     ├── deployment.yml
 │     │     ├── secret.yml
 │     │     └── service.yml
 │     ├── auth-api/
 │     │     ├── configmap.yml
 │     │     ├── deployment.yml
 │     │     ├── secret.yml
 │     │     └── service.yml
 │     ├── bff-api/
 │     │     ├── configmap.yml
 │     │     ├── deployment.yml
 │     │     ├── secret.yml
 │     │     └── service.yml
 │     ├── conteudo-api/
 │     │     ├── configmap.yml
 │     │     ├── deployment.yml
 │     │     ├── secret.yml
 │     │     └── service.yml
 │     ├── frontend/
 │     │     ├── deployment.yml
 │     │     └── service.yml
 │     └── pagamentos-api/
 │           ├── configmap.yml
 │           ├── deployment.yml
 │           ├── secret.yml
 │           └── service.yml
 └── namespace.yml
```

## 📜 Scripts PowerShell de suporte (Contêineres locais e Minikube)
```
scripts/
  ├── start-container.ps1
  ├── start-k8s.minikube.ps1
  ├── stop-container.ps1
  └── stop-k8s.minikube.ps1
```

## 📂 Estrutura do código fonte do Repositório
```
src/
 ├── backend/
 │     ├── alunos-api/
 │     ├── auth-api/
 │     ├── bff-api/
 │     ├── building-blocks/
 │     ├── conteudo-api/
 │     ├── pagamentos-api/
 │     └── MBA.Modulo5.sln
 │
 └── frontend/
        └── Angular App
```

## ▶️ Executando Localmente com Docker Compose
Antes de iniciar este tópico, segue como pré requisito ter instalado o Docker Desktop para que possa ser realizado o pull das imagens
Não recomendamos que seja realizado o carregamento individual dos contâineres pois existem pré requisitos necessários para o correto funcionamento do ecossistema. 

### Como carregar todo o ecossistema?
Abra um terminal PowerShell, navegue até a pasta raiz do projeto e execute a linha de comando abaixo:
```
./scripts/start-container.ps1
```

Para finalizar (parar todos os contâineres), execute o comando abaixo
```
./scripts/stop-container.ps1
```

## 🌐 Executando via Docker Hub
Exemplo de execução usando Auth API – altere a porta e o nome da imagem para executar outros microsserviços.
```
docker run --rm -p 5001:5001 jsouzalp/project5-auth-api:latest
```

Acesse:
http://localhost:5001/swagger

## 🧪 Testes Automatizados
Sempre que um pull-request de mudança no código-fonte de uma API é realizado para DEV ou PR (Alunos-API por exemplo) , são realizados os testes unitários da API modificada
Quando é realizado um pull-request para a main, são realizados os testes por completo da solução

## 🔄 Branch Strategy
feature/xpto → DEV → PR → MAIN

## 🤖 CI/CD
Cada microsserviço possui uma pipeline própria, permitindo build, testes e deploy independentes
Na pasta .github/ estão os arquivos de configuração de build e testes. 

## 🏷 Badges
![Auth API](https://github.com/jsouzalp/MBA-Modulo5/actions/workflows/auth-api.yml/badge.svg)
![BFF API](https://github.com/jsouzalp/MBA-Modulo5/actions/workflows/bff-api.yml/badge.svg)
![Conteúdo API](https://github.com/jsouzalp/MBA-Modulo5/actions/workflows/conteudo-api.yml/badge.svg)
![Alunos API](https://github.com/jsouzalp/MBA-Modulo5/actions/workflows/alunos-api.yml/badge.svg)
![Pagamentos API](https://github.com/jsouzalp/MBA-Modulo5/actions/workflows/pagamentos-api.yml/badge.svg)
![Frontend](https://github.com/jsouzalp/MBA-Modulo5/actions/workflows/frontend.yml/badge.svg)

## 🐳 Imagens no Docker Hub
As imagens Docker utilizam três tipos de tags:
- latest
- "<versão-semântica>" (extraída do csproj)
- "<commit-sha>" (para rastreabilidade)

Os repositórios criados estão disponíveis nos links abaixo
- https://hub.docker.com/r/jsouzalp/project5-auth-api
- https://hub.docker.com/r/jsouzalp/project5-bff-api
- https://hub.docker.com/r/jsouzalp/project5-conteudo-api
- https://hub.docker.com/r/jsouzalp/project5-alunos-api
- https://hub.docker.com/r/jsouzalp/project5-pagamentos-api
- https://hub.docker.com/r/jsouzalp/project5-frontend

## 🧰 Tecnologias
.NET 9, Angular, Docker, SQL Server, RabbitMQ, Redis, GitHub Actions, Minikube

## 📜 Licença
Projeto acadêmico do MBA DevXpert.
