@echo off

Echo "Navegando para camada ROOT"
cd ..\

Echo "Apagando migrations anteriores (SqlServer)..."
Del .\auth-api\src\Auth.Infrastructure\Migrations\SqlServer /Q
Del .\alunos-api\src\Alunos.Infrastructure\Migrations\SqlServer /Q
Del .\conteudo-api\src\Conteudo.Infrastructure\Migrations\SqlServer /Q
Del .\pagamentos-api\src\Pagamentos.Infrastructure\Migrations\SqlServer /Q

rem ----------------------------------------------------------------------------------------------
Echo "Criando Migration ALUNOS.API (SqlServer)"
dotnet ef migrations add InitialMigrationSqlServer --project .\alunos-api\src\Alunos.Infrastructure --startup-project .\alunos-api\src\Alunos.Api --context AlunoDbContext --output-dir Migrations\SqlServer

Echo "Criando Migration AUTH.API (SqlServer)"
dotnet ef migrations add InitialMigrationSqlServer --project .\auth-api\src\Auth.Infrastructure --startup-project .\auth-api\src\Auth.Api --context AuthDbContext --output-dir Migrations\SqlServer

Echo "Criando Migration CONTEUDO.API (SqlServer)"
dotnet ef migrations add InitialMigrationSqlServer --project .\conteudo-api\src\Conteudo.Infrastructure --startup-project .\conteudo-api\src\Conteudo.Api --context ConteudoDbContext --output-dir Migrations\SqlServer

Echo "Criando Migration PAGAMENTOS.API (SqlServer)"
dotnet ef migrations add InitialMigrationSqlServer --project .\pagamentos-api\src\Pagamentos.Infrastructure --startup-project .\pagamentos-api\src\Pagamentos.Api --context PagamentoContext --output-dir Migrations\SqlServer
rem ----------------------------------------------------------------------------------------------

rem ----------------------------------------------------------------------------------------------
Echo "Gerando script de BD -- ALUNO.API"
dotnet ef migrations script --project .\alunos-api\src\Alunos.Api --context AlunoDbContext > ..\..\data\0001_Aluno.Api.sql

Echo "Gerando script de BD -- AUTH.API"
dotnet ef migrations script --project .\auth-api\src\Auth.Api --context AuthDbContext > ..\..\data\0001_Auth.Api.sql

Echo "Gerando script de BD -- CONTEUDO.API"
dotnet ef migrations script --project .\conteudo-api\src\Conteudo.Api --context ConteudoDbContext > ..\..\data\0001_Conteudo.Api.sql

Echo "Gerando script de BD -- PAGAMENTOS.API"
dotnet ef migrations script --project .\pagamentos-api\src\Pagamentos.Api --context PagamentoContext > ..\..\data\0001_Pagamento.Api.sql
rem ----------------------------------------------------------------------------------------------

@echo "Migrations executadas"
@echo "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! IMPORTANTE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"
@echo ""
@echo "Revise os arquivos gerados. Normalmente esse recurso deixam linhas de output dos comandos e eh ALTAMENTE recomendavel que essa revisao seja feita!"
@echo ""
@echo "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! IMPORTANTE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"
pause
exit


