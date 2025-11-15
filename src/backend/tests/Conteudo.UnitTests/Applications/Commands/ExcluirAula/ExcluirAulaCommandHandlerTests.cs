using Conteudo.Application.Commands.ExcluirAula;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Data;
using Core.Mediator;
using Core.Messages;

namespace Conteudo.UnitTests.Applications.Commands.ExcluirAula;
public class ExcluirAulaCommandHandlerTests
{
    private readonly Mock<IAulaRepository> _aulas = new();
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private ExcluirAulaCommandHandler CriarSut()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _aulas.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        return new ExcluirAulaCommandHandler(_aulas.Object, _mediator.Object);
    }

    [Fact]
    public async Task Deve_retornar_invalido_quando_command_invalido()
    {
        var cmd = new ExcluirAulaCommand(Guid.NewGuid(), Guid.Empty);
        var sut = CriarSut();

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _aulas.Verify(r => r.ExcluirAulaAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_aula_nao_encontrada()
    {
        var cmd = new ExcluirAulaCommand(Guid.NewGuid(), Guid.NewGuid());
        var sut = CriarSut();

        _aulas.Setup(r => r.ObterPorIdAsync(cmd.CursoId, cmd.Id, true)).ReturnsAsync((Aula?)null);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_excluir_commit_e_retornar_true()
    {
        var cursoId = Guid.NewGuid();
        var aula = new Aula(cursoId, "A1", "d", 1, 10, "u", "V", true, "");

        var cmd = new ExcluirAulaCommand(cursoId, aula.Id);
        var sut = CriarSut();

        _aulas.Setup(r => r.ObterPorIdAsync(cmd.CursoId, cmd.Id, false))
              .ReturnsAsync(aula);

        var res = await sut.Handle(cmd, CancellationToken.None);

        _aulas.Verify(r => r.ExcluirAulaAsync(cmd.CursoId, cmd.Id), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Once);
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Never);

        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(true);
    }

    [Fact]
    public async Task Commit_false_deve_notificar_e_manter_Data_nula()
    {
        var cursoId = Guid.NewGuid();
        var aula = new Aula(cursoId, "A1", "d", 1, 10, "u", "V", true, "");

        var cmd = new ExcluirAulaCommand(cursoId, aula.Id);
        var sut = CriarSut();

        _aulas.Setup(r => r.ObterPorIdAsync(cmd.CursoId, aula.Id, false))
              .ReturnsAsync(aula);
        _uow.Setup(u => u.Commit()).ReturnsAsync(false);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _aulas.Verify(r => r.ExcluirAulaAsync(cmd.CursoId, cmd.Id), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Once);
    }
}
