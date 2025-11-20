using Conteudo.Domain.Entities;
using Conteudo.Infrastructure.Data;
using Conteudo.Infrastructure.Repositories;
using Conteudo.UnitTests.Repositories.Infra;
using Microsoft.Data.Sqlite;

namespace Conteudo.UnitTests.Repositories;
public class CategoriaRepositoryTests : IDisposable
{
    private readonly SqliteConnection _conn;
    private readonly ConteudoDbContext _ctx;
    private readonly CategoriaRepository _repo;

    public CategoriaRepositoryTests()
    {
        var (options, conn) = EfSqliteInMemoryFactory.Create();
        _conn = conn;
        _ctx = new ConteudoDbContext(options);
        _repo = new CategoriaRepository(_ctx);
    }

    [Fact]
    public async Task Adicionar_Atualizar_Obter_e_Listar_devem_funcionar()
    {
        var cat = new Categoria("Arquitetura", "Desc", "#123456", "", 0);
        _repo.Adicionar(cat);
        await _repo.UnitOfWork.Commit();

        cat = await _repo.ObterPorIdAsync(cat.Id, false);
        cat!.Nome.Should().Be("Arquitetura");

        cat.AtualizarInformacoes("Engenharia", "Nova", "#654321", iconeUrl: "i", ordem: 3);
        _repo.Atualizar(cat);
        await _repo.UnitOfWork.Commit();

        (await _repo.ObterPorIdAsync(cat.Id))!.Nome.Should().Be("Engenharia");

        var todas = (await _repo.ObterTodosAsync()).ToList();
        todas.Should().ContainSingle(c => c.Id == cat.Id);
    }

    [Fact]
    public async Task ExistePorNome_deve_usar_Like_podendo_aceitar_padrao()
    {
        var c1 = new Categoria("Arquitetura de Software", "d", "#111111", "", 0);
        var c2 = new Categoria("Data Architecture", "d", "#222222", "", 0);
        _ctx.Categorias.AddRange(c1, c2); await _ctx.SaveChangesAsync();

        (await _repo.ExistePorNome("Arquitetura de Software")).Should().BeTrue();
        (await _repo.ExistePorNome("%Arquit%")).Should().BeTrue();      // LIKE pattern
        (await _repo.ExistePorNome("Inexistente")).Should().BeFalse();
    }

    public void Dispose() => _conn.Dispose();
}
