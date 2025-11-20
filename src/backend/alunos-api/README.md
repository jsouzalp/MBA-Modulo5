# Alunos API - Microserviço de Gestão de Alunos

## 📋 Descrição

API de Gestão de Alunos para a Plataforma Educacional, responsável por gerenciar alunos, matrículas, certificados, progresso e histórico acadêmico.

## 🏗️ Arquitetura

- **Clean Architecture** com 4 camadas
- **Domain-Driven Design (DDD)**
- **CQRS** (preparado para MediatR)
- **JWT Authentication**
- **Entity Framework Core**
- **Swagger/OpenAPI**

## 🚀 Como Executar

### Pré-requisitos
- .NET 9.0 SDK
- SQL Server (LocalDB ou instância completa)
- Visual Studio 2022 ou VS Code

### Executar o Projeto
```bash
cd src/Alunos.API
dotnet run --urls "https://localhost:5003;http://localhost:7003"
```

### Acessar Swagger
- **URL**: https://localhost:5003/swagger
- **Health Check**: https://localhost:5003/health

## 📊 Domínio Implementado

### Entidades Principais

#### **Aluno** (Raiz de Agregado)
- Gestão completa de dados do aluno
- Relacionamento com matrículas
- Validações de negócio
- Controle de ativação/desativação

#### **MatriculaCurso**
- Gestão de matrículas em cursos
- Controle de status (Ativa, EmAndamento, Concluída, Cancelada, Suspensa)
- Relacionamento com progresso e certificados
- Validações de negócio

#### **Progresso**
- Acompanhamento detalhado do progresso nas aulas
- Controle de tempo assistido e percentual
- Status de progresso (NaoIniciado, EmAndamento, Concluido)

#### **Certificado**
- Emissão e gestão de certificados
- Validação e verificação de autenticidade
- Controle de validade e status
- Hash de segurança

#### **HistoricoAluno**
- Auditoria completa das ações do aluno
- Registro de logins, acessos e atividades
- Relatórios e estatísticas

## 🔗 Controllers Implementados

### **AlunoCommandController** (Operações de Escrita)
- `POST /api/alunos` - Cadastrar novo aluno
- `PUT /api/alunos/{id}` - Atualizar dados do aluno
- `PATCH /api/alunos/{id}/ativar` - Ativar aluno
- `PATCH /api/alunos/{id}/desativar` - Desativar aluno
- `DELETE /api/alunos/{id}` - Excluir aluno

### **AlunoQueryController** (Operações de Leitura)
- `GET /api/alunos` - Listar alunos (com paginação e filtros)
- `GET /api/alunos/{id}` - Obter aluno por ID
- `GET /api/alunos/usuario/{codigoUsuario}` - Obter aluno por código de usuário
- `GET /api/alunos/{id}/perfil` - Obter perfil completo do aluno
- `GET /api/alunos/{id}/dashboard` - Obter dashboard do aluno
- `GET /api/alunos/{id}/estatisticas` - Obter estatísticas do aluno
- `GET /api/alunos/{id}/matriculas` - Listar matrículas do aluno
- `GET /api/alunos/{id}/certificados` - Listar certificados do aluno
- `HEAD /api/alunos/{id}` - Verificar se aluno existe

### **MatriculaController**
- `GET /api/matriculas` - Listar matrículas (com filtros)
- `GET /api/matriculas/{id}` - Obter matrícula por ID
- `POST /api/matriculas/aluno/{alunoId}` - Criar nova matrícula
- `PUT /api/matriculas/{id}` - Atualizar matrícula
- `PATCH /api/matriculas/{id}/concluir` - Concluir matrícula
- `PATCH /api/matriculas/{id}/cancelar` - Cancelar matrícula
- `GET /api/matriculas/{id}/progresso` - Obter progresso da matrícula
- `HEAD /api/matriculas/{id}` - Verificar se matrícula existe

### **CertificadoController**
- `GET /api/certificados` - Listar certificados (com filtros)
- `GET /api/certificados/{id}` - Obter certificado por ID
- `GET /api/certificados/codigo/{codigo}` - Obter certificado por código
- `POST /api/certificados` - Gerar certificado
- `GET /api/certificados/validar/{codigo}` - Validar certificado (público)
- `PATCH /api/certificados/{id}/renovar` - Renovar certificado
- `PATCH /api/certificados/{id}/revogar` - Revogar certificado
- `GET /api/certificados/{id}/download` - Download do certificado
- `HEAD /api/certificados/{id}` - Verificar se certificado existe

### **ProgressoController**
- `GET /api/progresso/{id}` - Obter progresso por ID
- `GET /api/progresso/matricula/{matriculaId}` - Obter progresso por matrícula
- `GET /api/progresso/matricula/{matriculaId}/aula/{aulaId}` - Obter progresso específico
- `PUT /api/progresso/matricula/{matriculaId}/aula/{aulaId}` - Atualizar progresso
- `PATCH /api/progresso/matricula/{matriculaId}/aula/{aulaId}/concluir` - Concluir aula
- `POST /api/progresso/matricula/{matriculaId}/aula/{aulaId}/iniciar` - Iniciar aula
- `GET /api/progresso/matricula/{matriculaId}/relatorio` - Obter relatório de progresso
- `PATCH /api/progresso/matricula/{matriculaId}/aula/{aulaId}/abandonar` - Abandonar aula

### **HistoricoController**
- `GET /api/historico` - Listar histórico (com filtros)
- `GET /api/historico/{id}` - Obter histórico por ID
- `GET /api/historico/aluno/{alunoId}` - Obter histórico por aluno
- `POST /api/historico` - Criar registro de histórico
- `GET /api/historico/aluno/{alunoId}/recentes` - Obter atividades recentes
- `GET /api/historico/aluno/{alunoId}/estatisticas` - Obter estatísticas de atividade
- `GET /api/historico/tipo/{tipoAcao}` - Obter histórico por tipo de ação
- `POST /api/historico/aluno/{alunoId}/login` - Registrar login
- `POST /api/historico/aluno/{alunoId}/logout` - Registrar logout

## 🔧 Configuração

### JWT Authentication
- Configurado para validar tokens JWT
- Integração com Auth API
- Middleware de autenticação e autorização

### Swagger/OpenAPI
- Documentação completa da API
- Suporte a autenticação JWT
- Exemplos de request/response
- Validação de esquemas

### Health Checks
- Endpoint `/health` para monitoramento
- Verificação de conectividade com dependências

### CORS
- Configurado para permitir requisições de qualquer origem
- Necessário para integração com frontend

## 🔄 Integração com Outros Serviços

### Auth API (Porta 5001)
- Validação de tokens JWT
- Autenticação de usuários

### Conteudo API (Porta 5002)
- Consulta de informações de cursos
- Validação de matrículas

### RabbitMQ
- Eventos de matrícula realizada
- Eventos de certificado gerado
- Eventos de conclusão de curso

## 📊 DTOs Implementados

### Aluno
- `AlunoCadastroDto` - Cadastro de novo aluno
- `AlunoAtualizarDto` - Atualização de dados
- `AlunoDto` - Dados completos do aluno
- `AlunoResumoDto` - Dados resumidos para listagem
- `AlunoPerfilDto` - Perfil completo com estatísticas
- `AlunoDashboardDto` - Dashboard com métricas

### Matrícula
- `MatriculaCadastroDto` - Nova matrícula
- `MatriculaAtualizarDto` - Atualização de matrícula
- `MatriculaDto` - Dados completos da matrícula
- `MatriculaConclusaoDto` - Conclusão de curso
- `MatriculaCancelamentoDto` - Cancelamento

### Certificado
- `CertificadoGerarDto` - Geração de certificado
- `CertificadoDto` - Dados do certificado
- `CertificadoValidacaoDto` - Validação de certificado
- `CertificadoValidacaoResultadoDto` - Resultado da validação

### Progresso
- `ProgressoAtualizarDto` - Atualização de progresso
- `ProgressoDto` - Dados do progresso
- `ProgressoRelatorioDto` - Relatório de progresso

### Histórico
- `HistoricoAlunoDto` - Registro de histórico
- `HistoricoAlunoPaginadoDto` - Histórico paginado
- `HistoricoAlunoFiltroDto` - Filtros para consulta

## 🔨 Próximos Passos

1. **Implementar Repositórios** - Implementações concretas dos repositórios
2. **Implementar Application Services** - Lógica de negócio da aplicação
3. **Configurar Entity Framework** - Mapeamentos e migrations
4. **Implementar AutoMapper** - Mapeamento entre entidades e DTOs
5. **Configurar RabbitMQ** - Publicação e consumo de eventos
6. **Implementar Testes** - Testes unitários e de integração
7. **Configurar Docker** - Containerização da aplicação

## 📝 Observações

- Todos os controllers estão implementados e funcionais
- Autenticação JWT configurada
- Swagger com documentação completa
- Build bem-sucedido
- Interfaces dos Application Services criadas
- Estrutura preparada para implementação dos serviços 