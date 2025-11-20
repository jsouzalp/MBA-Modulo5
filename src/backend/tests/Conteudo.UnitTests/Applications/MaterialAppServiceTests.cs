using Conteudo.Application.Interfaces.Services;
using Conteudo.Application.Services;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using NSubstitute;

namespace Conteudo.UnitTests.Applications;

public class MaterialAppServiceTests
{
    private readonly IMaterialRepository _repo = Substitute.For<IMaterialRepository>();
    private readonly IMaterialAppService _svc;

    public MaterialAppServiceTests()
    {
        _svc = new MaterialAppService(_repo);
    }

    private static Material Novo(Guid aulaId, string nome) =>
        new Material(aulaId, nome, "desc", "PDF", "https://u", false, 100, ".pdf", 1);

    [Fact]
    public async Task ObterPorId_deve_mapear_para_dto_quando_existir()
    {
        var mat = Novo(Guid.NewGuid(), "Slides");
        _repo.ObterPorIdAsync(mat.Id).Returns(mat);

        var dto = await _svc.ObterPorIdAsync(mat.Id);

        dto.Should().NotBeNull();
        dto!.Nome.Should().Be("Slides");
        await _repo.Received(1).ObterPorIdAsync(mat.Id);
    }

    [Fact]
    public async Task ObterPorAulaId_deve_retornar_todos_e_preservar_ordem()
    {
        var aulaId = Guid.NewGuid();
        var items = new List<Material> { Novo(aulaId, "M1"), Novo(aulaId, "M2") };
        _repo.ObterPorAulaIdAsync(aulaId).Returns(items);

        var dtos = (await _svc.ObterPorAulaIdAsync(aulaId)).ToList();

        dtos.Should().HaveCount(2);
        dtos.Select(x => x.Nome).Should().ContainInOrder("M1", "M2");
        await _repo.Received(1).ObterPorAulaIdAsync(aulaId);
    }

    [Fact]
    public async Task Listas_auxiliares_devem_chamar_metodos_corretos_do_repo()
    {
        _repo.ObterAtivosAsync().Returns(new List<Material> { Novo(Guid.NewGuid(), "A") });
        _repo.ObterAtivosPorAulaIdAsync(Arg.Any<Guid>()).Returns(new List<Material> { Novo(Guid.NewGuid(), "B") });
        _repo.ObterObrigatoriosPorAulaIdAsync(Arg.Any<Guid>()).Returns(new List<Material> { Novo(Guid.NewGuid(), "C") });

        (await _svc.ObterAtivosAsync()).Should().HaveCount(1);
        (await _svc.ObterAtivosPorAulaIdAsync(Guid.NewGuid())).Should().HaveCount(1);
        (await _svc.ObterObrigatoriosPorAulaIdAsync(Guid.NewGuid())).Should().HaveCount(1);

        await _repo.Received(1).ObterAtivosAsync();
        await _repo.Received(1).ObterAtivosPorAulaIdAsync(Arg.Any<Guid>());
        await _repo.Received(1).ObterObrigatoriosPorAulaIdAsync(Arg.Any<Guid>());
    }
}
