# Auth API - Microsserviço de Autenticação

## 📋 Visão Geral

A Auth API é um microsserviço responsável pela autenticação e autorização de usuários na plataforma educacional. Implementa Clean Architecture com ASP.NET Core, Entity Framework Core e JWT para autenticação stateless.

## 🏗️ Arquitetura

### Clean Architecture Layers

```
├── Auth.API/              # Camada de Apresentação (Controllers, Models)
├── Auth.Application/      # Camada de Aplicação (Services, DTOs, Interfaces)
├── Auth.Domain/          # Camada de Domínio (Entities, Events)
├── Auth.Infrastructure/   # Camada de Infraestrutura (Data, External Services)
└── tests/                # Testes (Unit, Integration)
```

### Tecnologias Utilizadas

- **ASP.NET Core 9.0** - Framework web
- **Entity Framework Core** - ORM para acesso a dados
- **ASP.NET Core Identity** - Sistema de autenticação
- **JWT Bearer** - Tokens de autenticação
- **SQLite** - Banco de dados (desenvolvimento)
- **Swagger/OpenAPI** - Documentação da API

## 🚀 Como Executar

### Pré-requisitos

- .NET 9.0 SDK
- Visual Studio 2022 ou VS Code

### Executando Localmente

1. **Clone o repositório**
   ```bash
   git clone <repository-url>
   cd src/backend/auth-api
   ```

2. **Restaurar dependências**
   ```bash
   dotnet restore
   ```

3. **Executar a aplicação**
   ```bash
   dotnet run --project src/Auth.API
   ```

4. **Acessar a API**
   - API: `https://localhost:5002`
   - Swagger: `https://localhost:5002/swagger`
   - Health Check: `https://localhost:5002/health`

## 📚 Endpoints

### Autenticação

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/auth/registro` | Registrar novo usuário |
| POST | `/api/auth/login` | Autenticar usuário |
| POST | `/api/auth/refresh-token` | Renovar token de acesso |

### Exemplos de Uso

#### Registro de Usuário
```http
POST /api/auth/registro
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@teste.com",
  "senha": "MinhaSenh@123",
  "dataNascimento": "1995-05-15T00:00:00",
  "ehAdministrador": false
}
```

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "joao@teste.com",
  "senha": "MinhaSenh@123"
}
```

#### Renovar Token
```http
POST /api/auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "seu_refresh_token_aqui"
}
```

## 🔧 Configuração

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=auth.db"
  },
  "JwtSettings": {
    "SecretKey": "sua_chave_secreta_aqui",
    "Issuer": "Auth.API",
    "Audience": "Auth.API.Users",
    "ExpiryMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

### Variáveis de Ambiente

- `ASPNETCORE_ENVIRONMENT` - Ambiente (Development/Production)
- `ConnectionStrings__DefaultConnection` - String de conexão do banco
- `JwtSettings__SecretKey` - Chave secreta para JWT

## 🧪 Testes

### Executar Testes Unitários
```bash
dotnet test tests/Auth.UnitTests
```

### Executar Testes de Integração
```bash
dotnet test tests/Auth.IntegrationTests
```

### Executar Todos os Testes
```bash
dotnet test
```

## 📊 Monitoramento

### Health Checks
- Endpoint: `/health`
- Verifica: Conectividade com banco de dados

### Logs
- Estruturados com Serilog
- Níveis configuráveis por ambiente
- Correlação de requests via CorrelationId

## 🔐 Segurança

### Autenticação JWT
- Tokens com expiração configurável
- Refresh tokens para renovação
- Claims baseadas em roles

### Roles do Sistema
- **Administrador**: Acesso total ao sistema
- **Usuario**: Acesso básico de aluno

### Usuário Padrão
- **Email**: admin@auth.api
- **Senha**: Admin@123
- **Role**: Administrador

## 🐳 Docker

### Build da Imagem
```bash
docker build -t auth-api .
```

### Executar Container
```bash
docker run -p 5002:8080 auth-api
```

## 📈 Performance

### Otimizações Implementadas
- Connection pooling no Entity Framework
- Async/await em todas as operações I/O
- Caching de configurações JWT
- Validação eficiente com Data Annotations

## 🔄 Integração com Outros Microsserviços

### Eventos Publicados
- `UserRegisteredEvent` - Quando um usuário é registrado
- `UserLoggedInEvent` - Quando um usuário faz login

### Comunicação
- **Síncrona**: HTTP/REST via BFF
- **Assíncrona**: RabbitMQ (planejado)

## 📝 Logs de Desenvolvimento

### Versão 1.0.0
- ✅ Implementação da Clean Architecture
- ✅ Autenticação JWT
- ✅ ASP.NET Core Identity
- ✅ Swagger Documentation
- ✅ Health Checks
- ✅ Separação de modelos (Requests/Responses)
- ✅ Tratamento de erros
- ✅ Configurações por ambiente

### Próximas Funcionalidades
- [ ] Integração com RabbitMQ
- [ ] Testes automatizados completos
- [ ] Implementação de rate limiting
- [ ] Auditoria de login
- [ ] Two-factor authentication

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes. 