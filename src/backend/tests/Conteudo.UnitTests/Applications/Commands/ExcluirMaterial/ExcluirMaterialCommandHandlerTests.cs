using Conteudo.Application.Commands.ExcluirMaterial;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Data;
using Core.Mediator;
using Core.Messages;

namespace Conteudo.UnitTests.Applications.Commands.ExcluirMaterial;
public class ExcluirMaterialCommandHandlerTests
{
    private readonly Mock<IMaterialRepository> _materiais = new();
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private ExcluirMaterialCommandHandler CriarSut()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _materiais.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        return new ExcluirMaterialCommandHandler(_materiais.Object, _mediator.Object);
    }

    [Fact]
    public async Task Deve_retornar_invalido_quando_command_invalido()
    {
        var cmd = new ExcluirMaterialCommand(Guid.Empty);
        var sut = CriarSut();

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _materiais.Verify(r => r.ExcluirMaterialAsync(It.IsAny<Guid>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_material_nao_encontrado()
    {
        var cmd = new ExcluirMaterialCommand(Guid.NewGuid());
        var sut = CriarSut();

        _materiais.Setup(r => r.ObterPorIdAsync(cmd.Id)).ReturnsAsync((Material?)null);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_excluir_commit_e_retornar_true()
    {
        var aulaId = Guid.NewGuid();
        var material = new Material(aulaId, "M", "d", "PDF", "u", false, 0, ".pdf", 0);

        var cmd = new ExcluirMaterialCommand(material.Id);
        var sut = CriarSut();

        _materiais.Setup(r => r.ObterPorIdAsync(cmd.Id)).ReturnsAsync(material);

        var res = await sut.Handle(cmd, CancellationToken.None);

        _materiais.Verify(r => r.ExcluirMaterialAsync(cmd.Id), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Once);
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Never);

        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(true);
    }

    [Fact]
    public async Task Commit_false_deve_notificar_e_manter_Data_nula()
    {
        var cmd = new ExcluirMaterialCommand(Guid.NewGuid());
        var sut = CriarSut();

        _materiais.Setup(r => r.ObterPorIdAsync(cmd.Id)).ReturnsAsync(new Material(Guid.NewGuid(), "M", "d", "PDF", "u", false, 0, ".pdf", 0));
        _uow.Setup(u => u.Commit()).ReturnsAsync(false);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Once);
    }
}
