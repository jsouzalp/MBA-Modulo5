using Alunos.Domain.Entities;
using Alunos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Alunos.API.Helpers;

[ExcludeFromCodeCoverage]
public static class DbMigrationHelpers
{
    public static void UseDbMigrationHelper(this WebApplication app)
    {
        EnsureSeedData(app).Wait();
    }

    public static async Task EnsureSeedData(WebApplication application)
    {
        var service = application.Services.CreateScope().ServiceProvider;
        await EnsureSeedData(service);
    }

    private static async Task EnsureSeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AlunoDbContext>();
        var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        if (env.IsDevelopment() || env.IsEnvironment("Docker"))
        {
            await EnsureSeedData(context);
        }
    }

    private static async Task EnsureSeedData(AlunoDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!context.Alunos.Any())
        {
            Guid alunoUm = Guid.Parse("06b1b8f1-f079-4048-9c8d-190c8056ea60");
            Guid alunoDois = Guid.Parse("ca39e314-c960-42bc-9c9d-3cad9b589a8d");

            await InserirAluno(context, alunoUm, "Aluno de Numero UM", "aluno1@auth.api", "12345678911", new DateTime(1973, 1, 1), "Masculino");
            await InserirAluno(context, alunoDois, "Aluno de Numero DOIS", "aluno2@auth.api", "98765432111", new DateTime(2000, 1, 1), "Feminino");
        }
    }

    private static async Task InserirAluno(AlunoDbContext context, Guid alunoId, string nome, string email, string cpf, DateTime dataNascimento, string genero)
    {
        Aluno aluno = new(alunoId, nome, email, cpf, dataNascimento, genero, "Rio de Janeiro", "RJ", "00000000", "");
        aluno.DefinirId(alunoId);
        aluno.AtivarAluno();

        Guid cursoIdUm = Guid.Parse("f1a2b3c4-d5e6-7f8a-9b0c-1d2e3f4a5b6c");
        aluno.MatricularAlunoEmCurso(cursoIdUm, "C# Avançado", 499.90m, $"Observação Aluno {nome} - Curso 1-C# Avançado");

        MatriculaCurso matriculaUm = aluno.MatriculasCursos.ToArray()[0];
        aluno.AtualizarPagamentoMatricula(matriculaUm.Id);
        aluno.RegistrarHistoricoAprendizado(matriculaUm.Id, Guid.Parse("9be503ca-83fb-41cb-98e4-8f0ae98692a0"), $"Conteúdo da aula 1 do curso {matriculaUm.NomeCurso}", 20, DateTime.UtcNow);
        aluno.RegistrarHistoricoAprendizado(matriculaUm.Id, Guid.Parse("c55fd2e3-9a07-4b1d-8b35-237c12712ad4"), $"Conteúdo da aula 2 do curso {matriculaUm.NomeCurso}", 40, DateTime.UtcNow);
        aluno.RegistrarHistoricoAprendizado(matriculaUm.Id, Guid.Parse("fbe91473-7e59-414b-90ef-c9a13b3c24ec"), $"Conteúdo da aula 3 do curso {matriculaUm.NomeCurso}", 125, DateTime.UtcNow);
        aluno.ConcluirCurso(matriculaUm.Id);
        aluno.RequisitarCertificadoConclusao(matriculaUm.Id, 10, $"/var/mnt/certificados/{aluno.Id}_{matriculaUm.Id}.pdf", "Curso Online");

        Guid cursoIdDois = Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d");
        aluno.MatricularAlunoEmCurso(cursoIdDois, "SQL Server do Zero ao Avançado", 499.90m, $"Observação Aluno {nome} - Curso 2-SQL Server do Zero ao Avançado");

        MatriculaCurso matriculaDois = aluno.MatriculasCursos.ToArray()[1];
        aluno.AtualizarPagamentoMatricula(matriculaDois.Id);
        aluno.RegistrarHistoricoAprendizado(matriculaDois.Id, Guid.Parse("84d09a65-8ac1-4bde-83a4-8533ab3b97a4"), $"Conteúdo da aula 1 do curso {matriculaDois.NomeCurso}", 20, DateTime.UtcNow);
        aluno.RegistrarHistoricoAprendizado(matriculaDois.Id, Guid.Parse("6557645c-5879-4120-a6ed-a5349a3701c8"), $"Conteúdo da aula 2 do curso {matriculaDois.NomeCurso}", 60, null);
        aluno.RegistrarHistoricoAprendizado(matriculaDois.Id, Guid.Parse("db77ee62-b666-47d4-8e5b-c651c81e7fac"), $"Conteúdo da aula 3 do curso {matriculaDois.NomeCurso}", 90, null);

        context.Alunos.Add(aluno);
        await context.SaveChangesAsync();
    }
}
