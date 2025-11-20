using Conteudo.Domain.Entities;
using Conteudo.Domain.ValueObjects;
using Conteudo.Infrastructure.Data;
using Conteudo.Infrastructure.Repositories;
using Conteudo.UnitTests.Repositories.Infra;
using Microsoft.Data.Sqlite;

namespace Conteudo.UnitTests.Repositories;
public class AulaRepositoryTests : IDisposable
{
    private readonly SqliteConnection _conn;
    private readonly ConteudoDbContext _ctx;
    private readonly AulaRepository _repo;

    public AulaRepositoryTests()
    {
        var (options, conn) = EfSqliteInMemoryFactory.Create();
        _conn = conn;
        _ctx = new ConteudoDbContext(options);
        _repo = new AulaRepository(_ctx);
    }

    private static ConteudoProgramatico VO() =>
        new("r", "d", "o", "pr", "pa", "m", "r", "a", "b");

    private Curso NovoCurso(Guid? categoriaId = null)
        => new("Curso X", 100, VO(), 10, "Básico", "Instrutor", 30, "", null, categoriaId);

    private Aula NovaAula(Guid cursoId, int numero = 1, string nome = "Aula 1")
        => new(cursoId, nome, "desc", numero, 30, "url", "Vídeo", true, "");

    [Fact]
    public async Task Cadastrar_ObterPorId_e_Excluir_devem_funcionar()
    {
        var curso = NovoCurso(); _ctx.Cursos.Add(curso); await _ctx.SaveChangesAsync();

        var aula = NovaAula(curso.Id, 1);
        await _repo.CadastrarAulaAsync(aula);
        await _repo.UnitOfWork.Commit();

        var carregada = await _repo.ObterPorIdAsync(curso.Id, aula.Id);
        carregada.Should().NotBeNull();
        carregada!.Numero.Should().Be(1);

        await _repo.ExcluirAulaAsync(curso.Id, aula.Id);
        await _repo.UnitOfWork.Commit();

        (await _repo.ObterPorIdAsync(curso.Id, aula.Id)).Should().BeNull();
    }

    [Fact]
    public async Task ObterTodos_e_ObterPorCursoId_devem_respeitar_cursoId_e_includeMateriais()
    {
        var curso = NovoCurso(); _ctx.Cursos.Add(curso); await _ctx.SaveChangesAsync();
        var aula = NovaAula(curso.Id, 1);
        _ctx.Aulas.Add(aula);

        // materiais para testar Include
        _ctx.Materiais.Add(new Material(aula.Id, "M1", "d", "PDF", "u", false, 1, ".pdf", 1));
        _ctx.Materiais.Add(new Material(aula.Id, "M2", "d", "PDF", "u", true, 1, ".pdf", 2));
        await _ctx.SaveChangesAsync();

        var semInclude = (await _repo.ObterTodosAsync(curso.Id)).ToList();
        semInclude.Should().HaveCount(1);
        semInclude[0].Materiais.Should().BeNullOrEmpty();

        var comInclude = (await _repo.ObterPorCursoIdAsync(curso.Id, includeMateriais: true)).ToList();
        comInclude.Should().HaveCount(1);
        comInclude[0].Materiais.Should().NotBeNull().And.HaveCount(2);
    }

    [Fact]
    public async Task Publicar_e_Despublicar_devem_alterar_flags_e_data()
    {
        var curso = NovoCurso(); _ctx.Cursos.Add(curso); await _ctx.SaveChangesAsync();
        var aula = NovaAula(curso.Id, 1);
        _ctx.Aulas.Add(aula); await _ctx.SaveChangesAsync();

        await _repo.PublicarAulaAsync(curso.Id, aula.Id);
        await _repo.UnitOfWork.Commit();

        var publicadas = (await _repo.ObterPublicadasPorCursoIdAsync(curso.Id)).ToList();
        publicadas.Should().ContainSingle(a => a.Id == aula.Id && a.IsPublicada);

        await _repo.DespublicarAulaAsync(curso.Id, aula.Id);
        await _repo.UnitOfWork.Commit();

        (await _repo.ObterPublicadasAsync()).Should().NotContain(a => a.Id == aula.Id);
    }

    [Fact]
    public async Task ExistePorNumero_deve_considerar_excludeId()
    {
        var curso = NovoCurso(); _ctx.Cursos.Add(curso); await _ctx.SaveChangesAsync();
        var a1 = NovaAula(curso.Id, 1); var a2 = NovaAula(curso.Id, 2);
        _ctx.Aulas.AddRange(a1, a2); await _ctx.SaveChangesAsync();

        (await _repo.ExistePorNumeroAsync(curso.Id, 1)).Should().BeTrue();
        (await _repo.ExistePorNumeroAsync(curso.Id, 1, excludeId: a1.Id)).Should().BeFalse();
    }

    public void Dispose() => _conn.Dispose();
}
