using Conteudo.Application.Commands.AtualizarMaterial;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Data;
using Core.Mediator;
using Core.Messages;

namespace Conteudo.UnitTests.Applications.Commands.AtualizarMaterial;
public class AtualizarMaterialCommandHandlerTests
{
    private readonly Mock<IMaterialRepository> _materiais = new();
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private AtualizarMaterialCommandHandler CriarSut()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _materiais.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        return new AtualizarMaterialCommandHandler(_materiais.Object, _mediator.Object);
    }

    private static Material MaterialExistente(Guid aulaId) =>
        new(aulaId, "Antigo", "d", "PDF", "u", false, 1, ".pdf", 1);

    [Fact]
    public async Task Deve_retornar_invalido_quando_command_invalido()
    {
        var cmd = new AtualizarMaterialCommand(
            cursoId: Guid.NewGuid(),
            id: Guid.Empty,         // inválido
            nome: "",               // inválido
            descricao: "",          // inválido
            tipoMaterial: "",       // inválido
            url: "",                // inválido
            isObrigatorio: true,
            tamanhoBytes: -1,       // inválido
            extensao: ".pdf",
            ordem: -5               // inválido
        );

        var sut = CriarSut();

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _materiais.Verify(r => r.AtualizarMaterialAsync(It.IsAny<Material>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_material_nao_encontrado()
    {
        var cmd = new AtualizarMaterialCommand(Guid.NewGuid(), Guid.NewGuid(), "n", "d", "PDF", "u", true, 1, ".pdf", 1);
        var sut = CriarSut();

        _materiais.Setup(r => r.ObterPorIdAsync(cmd.Id)).ReturnsAsync((Material?)null);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_nome_duplicado_na_mesma_aula()
    {
        var aulaId = Guid.NewGuid();
        var existente = MaterialExistente(aulaId);
        var cmd = new AtualizarMaterialCommand(Guid.NewGuid(), existente.Id, "Duplicado", "d", "PDF", "u", true, 1, ".pdf", 1);
        var sut = CriarSut();

        _materiais.Setup(r => r.ObterPorIdAsync(cmd.Id)).ReturnsAsync(existente);
        _materiais.Setup(r => r.ExistePorNomeAsync(aulaId, cmd.Nome, cmd.Id)).ReturnsAsync(true);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _materiais.Verify(r => r.AtualizarMaterialAsync(It.IsAny<Material>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_atualizar_material_e_commit_e_retornar_true()
    {
        var aulaId = Guid.NewGuid();
        var existente = MaterialExistente(aulaId);
        var cmd = new AtualizarMaterialCommand(Guid.NewGuid(), existente.Id, "Novo", "Nova desc", "PDF", "url", false, 2048, ".pdf", 3);
        var sut = CriarSut();

        _materiais.Setup(r => r.ObterPorIdAsync(cmd.Id)).ReturnsAsync(existente);
        _materiais.Setup(r => r.ExistePorNomeAsync(aulaId, cmd.Nome, cmd.Id)).ReturnsAsync(false);

        Material? atualizado = null;
        _materiais.Setup(r => r.AtualizarMaterialAsync(It.IsAny<Material>()))
                  .Callback<Material>(m => atualizado = m)
                  .Returns((Material m) => Task.FromResult(m));

        var res = await sut.Handle(cmd, CancellationToken.None);

        atualizado.Should().NotBeNull();
        atualizado!.Nome.Should().Be("Novo");
        atualizado.Descricao.Should().Be("Nova desc");
        atualizado.Ordem.Should().Be(3);

        _uow.Verify(u => u.Commit(), Times.Once);
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Never);

        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(true);
    }
}
