using Conteudo.Application.Interfaces.Services;
using Conteudo.Application.Services;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using NSubstitute;

namespace Conteudo.UnitTests.Applications;
public class AulaAppServiceTests
{
    private readonly IAulaRepository _repo = Substitute.For<IAulaRepository>();
    private readonly IAulaAppService _svc;

    public AulaAppServiceTests()
    {
        _svc = new AulaAppService(_repo);
    }

    private static Aula Nova(Guid cursoId, int numero, string nome = "Aula X") =>
        new Aula(cursoId, nome, "desc", numero, 30, "https://v", "Vídeo", true, "");

    [Fact]
    public async Task ObterTodos_deve_respeitar_includeMateriais()
    {
        var cursoId = Guid.NewGuid();
        var aulas = new List<Aula> { Nova(cursoId, 1), Nova(cursoId, 2) };
        _repo.ObterTodosAsync(cursoId, true).Returns(aulas);

        var dtos = (await _svc.ObterTodosAsync(cursoId, includeMateriais: true)).ToList();

        dtos.Should().HaveCount(2);
        await _repo.Received(1).ObterTodosAsync(cursoId, true);
    }

    [Fact]
    public async Task ObterPorId_deve_mapear_para_dto()
    {
        var cursoId = Guid.NewGuid();
        var a = Nova(cursoId, 1, "Introdução");
        _repo.ObterPorIdAsync(cursoId, a.Id, false).Returns(a);

        var dto = await _svc.ObterPorIdAsync(cursoId, a.Id);

        dto.Should().NotBeNull();
        dto!.Nome.Should().Be("Introdução");
        await _repo.Received(1).ObterPorIdAsync(cursoId, a.Id, false);
    }

    [Fact]
    public async Task ObterPublicadas_deve_respeitar_includeMateriais()
    {
        var aulas = new List<Aula> { Nova(Guid.NewGuid(), 1) };
        _repo.ObterPublicadasAsync(true).Returns(aulas);

        var dtos = await _svc.ObterPublicadasAsync(includeMateriais: true);

        dtos.Should().HaveCount(1);
        await _repo.Received(1).ObterPublicadasAsync(true);
    }
}
