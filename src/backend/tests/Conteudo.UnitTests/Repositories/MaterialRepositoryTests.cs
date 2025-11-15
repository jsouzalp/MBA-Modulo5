using Conteudo.Domain.Entities;
using Conteudo.Domain.ValueObjects;
using Conteudo.Infrastructure.Data;
using Conteudo.Infrastructure.Repositories;
using Conteudo.UnitTests.Repositories.Infra;
using Microsoft.Data.Sqlite;

namespace Conteudo.UnitTests.Repositories;
public class MaterialRepositoryTests : IDisposable
{
    private readonly SqliteConnection _conn;
    private readonly ConteudoDbContext _ctx;
    private readonly MaterialRepository _repo;

    public MaterialRepositoryTests()
    {
        var (options, conn) = EfSqliteInMemoryFactory.Create();
        _conn = conn;
        _ctx = new ConteudoDbContext(options);
        _repo = new MaterialRepository(_ctx);
    }

    private static ConteudoProgramatico VO() =>
        new("r", "d", "o", "pr", "pa", "m", "r", "a", "b");

    private (Curso curso, Aula aula) CursoEAula()
    {
        var curso = new Curso("Curso", 100, VO(), 10, "Básico", "Instrutor", 10, "", null, null);
        _ctx.Cursos.Add(curso); _ctx.SaveChanges();
        var aula = new Aula(curso.Id, "A1", "d", 1, 30, "url", "Vídeo", true, "");
        _ctx.Aulas.Add(aula); _ctx.SaveChanges();
        return (curso, aula);
    }

    [Fact]
    public async Task Cadastrar_Atualizar_Obter_e_Excluir_devem_funcionar()
    {
        var (_, aula) = CursoEAula();
        var mat = new Material(aula.Id, "M1", "d", "PDF", "u", false, 1, ".pdf", 1);

        await _repo.CadastrarMaterialAsync(mat);
        await _repo.UnitOfWork.Commit();

        var carregado = await _repo.ObterPorIdAsync(mat.Id);
        carregado!.Nome.Should().Be("M1");

        carregado.AtualizarInformacoes("M1+", "d2", "PDF", "u2", true, 2_000, ".pdf", 2);
        await _repo.AtualizarMaterialAsync(carregado);
        await _repo.UnitOfWork.Commit();

        (await _repo.ObterPorIdAsync(mat.Id))!.Nome.Should().Be("M1+");

        await _repo.ExcluirMaterialAsync(mat.Id);
        await _repo.UnitOfWork.Commit();

        (await _repo.ObterPorIdAsync(mat.Id)).Should().BeNull();
    }

    [Fact]
    public async Task Consultas_por_aula_ativos_e_obrigatorios_devem_refletir_flags()
    {
        var (_, aula) = CursoEAula();
        var m1 = new Material(aula.Id, "M1", "d", "PDF", "u1", false, 1, ".pdf", 1);
        var m2 = new Material(aula.Id, "M2", "d", "PDF", "u2", true, 1, ".pdf", 2);
        var m3 = new Material(aula.Id, "M3", "d", "PDF", "u3", false, 1, ".pdf", 3);
        m3.Desativar();
        _ctx.Materiais.AddRange(m1, m2, m3); await _ctx.SaveChangesAsync();

        (await _repo.ObterPorAulaIdAsync(aula.Id)).Should().HaveCount(3);
        (await _repo.ObterAtivosAsync()).Should().HaveCount(2);
        (await _repo.ObterAtivosPorAulaIdAsync(aula.Id)).Should().HaveCount(2);
        (await _repo.ObterObrigatoriosPorAulaIdAsync(aula.Id)).Select(x => x.Nome)
            .Should().BeEquivalentTo(new[] { "M2" });
    }

    [Fact]
    public async Task ExistePorNome_deve_considerar_excludeId()
    {
        var (_, aula) = CursoEAula();
        var m1 = new Material(aula.Id, "Dup", "d", "PDF", "u1", false, 1, ".pdf", 1);
        var m2 = new Material(aula.Id, "Outro", "d", "PDF", "u2", false, 1, ".pdf", 2);
        _ctx.Materiais.AddRange(m1, m2); await _ctx.SaveChangesAsync();

        (await _repo.ExistePorNomeAsync(aula.Id, "Dup")).Should().BeTrue();
        (await _repo.ExistePorNomeAsync(aula.Id, "Dup", excludeId: m1.Id)).Should().BeFalse();
    }

    public void Dispose() => _conn.Dispose();
}
