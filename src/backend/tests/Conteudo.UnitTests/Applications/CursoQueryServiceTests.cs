using Conteudo.Application.Interfaces.Services;
using Conteudo.Application.Services;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Domain.ValueObjects;
using NSubstitute;

namespace Conteudo.UnitTests.Applications;
public class CursoQueryServiceTests
{
    private readonly ICursoRepository _repo = Substitute.For<ICursoRepository>();
    private readonly ICursoQuery _svc;

    public CursoQueryServiceTests()
    {
        _svc = new CursoQueryService(_repo);
    }

    private static ConteudoProgramatico VO() =>
        new("r", "d", "o", "pr", "pa", "m", "r", "a", "b");

    private static Curso Novo(string nome, Guid? categoriaId = null) =>
        new Curso(nome, 100, VO(), 10, "Básico", "Instrutor", 10, "", null, categoriaId);

    [Fact]
    public async Task ObterPorId_deve_respeitar_includeAulas_e_mapear_para_dto()
    {
        var c = Novo("Curso X");
        _repo.ObterPorIdAsync(c.Id, true).Returns(c);

        var dto = await _svc.ObterPorIdAsync(c.Id, includeAulas: true);

        dto.Should().NotBeNull();
        dto!.Nome.Should().Be("Curso X");
        await _repo.Received(1).ObterPorIdAsync(c.Id, true);
    }

    [Fact]
    public async Task ObterPorCategoriaId_deve_retornar_lista_mapeada()
    {
        var cat = Guid.NewGuid();
        var lista = new List<Curso> { Novo("C1", cat), Novo("C2", cat) };
        _repo.ObterPorCategoriaIdAsync(cat, false).Returns(lista);

        var dtos = (await _svc.ObterPorCategoriaIdAsync(cat)).ToList();

        dtos.Should().HaveCount(2);
        dtos.Select(x => x.Nome).Should().BeEquivalentTo("C1", "C2");
        await _repo.Received(1).ObterPorCategoriaIdAsync(cat, false);
    }

    // Observação: não testei ObterTodos(CursoFilter) porque o tipo de filtro/PagedResult está em outro pacote.
    // Quando você disponibilizar essas classes no projeto de testes, adiciono um caso validando paginação e mapeamento.
}
