using Conteudo.Application.Interfaces.Services;
using Conteudo.Application.Services;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using NSubstitute;

namespace Conteudo.UnitTests.Applications;

public class CategoriaAppServiceTests
{
    private readonly ICategoriaRepository _repo = Substitute.For<ICategoriaRepository>();
    private readonly ICategoriaAppService _svc;

    public CategoriaAppServiceTests()
    {
        _svc = new CategoriaAppService(_repo);
    }

    [Fact]
    public async Task ObterTodasCategorias_deve_retornar_dtos_mapeados()
    {
        _repo.ObterTodosAsync().Returns(new List<Categoria>
        {
            new Categoria("Arq", "d", "#123", "", 0),
            new Categoria("Data", "d", "#456", "", 1)
        });

        var dtos = (await _svc.ObterTodasCategoriasAsync()).ToList();

        dtos.Should().HaveCount(2);
        dtos.Select(x => x.Nome).Should().BeEquivalentTo("Arq", "Data");
        await _repo.Received(1).ObterTodosAsync();
    }

    [Fact]
    public async Task ObterPorId_deve_mapear_para_dto_quando_existir()
    {
        var cat = new Categoria("Arq", "d", "#123", "", 0);
        _repo.ObterPorIdAsync(cat.Id).Returns(cat);

        var dto = await _svc.ObterPorIdAsync(cat.Id);

        dto.Should().NotBeNull();
        dto!.Nome.Should().Be("Arq");
        await _repo.Received(1).ObterPorIdAsync(cat.Id);
    }
}
