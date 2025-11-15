using Conteudo.Application.Commands.DespublicarAula;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Data;
using Core.Mediator;
using Core.Messages;

namespace Conteudo.UnitTests.Applications.Commands.DespublicarAula;
public class DespublicarAulaCommandHandlerTests
{
    private readonly Mock<IAulaRepository> _aulas = new();
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private DespublicarAulaCommandHandler CriarSut()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _aulas.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        return new DespublicarAulaCommandHandler(_aulas.Object, _mediator.Object);
    }

    [Fact]
    public async Task Deve_notificar_quando_aula_nao_existir()
    {
        var cmd = new DespublicarAulaCommand(Guid.NewGuid(), Guid.NewGuid());
        var sut = CriarSut();

        _aulas.Setup(r => r.ObterPorIdAsync(cmd.CursoId, cmd.Id, true)).ReturnsAsync((Aula?)null);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _aulas.Verify(r => r.DespublicarAulaAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_despublicar_commit_e_retornar_Id()
    {
        var cmd = new DespublicarAulaCommand(Guid.NewGuid(), Guid.NewGuid());
        var sut = CriarSut();

        _aulas.Setup(r => r.ObterPorIdAsync(cmd.CursoId, cmd.Id, false)).ReturnsAsync(new Aula(cmd.CursoId, "A", "d", 1, 10, "u", "V", true, ""));

        var res = await sut.Handle(cmd, CancellationToken.None);

        _aulas.Verify(r => r.DespublicarAulaAsync(cmd.CursoId, cmd.Id), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Once);

        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(cmd.Id);
    }

    [Fact]
    public async Task Excecao_deve_retornar_invalido_e_nao_commit()
    {
        var cmd = new DespublicarAulaCommand(Guid.NewGuid(), Guid.NewGuid());
        var sut = CriarSut();

        _aulas.Setup(r => r.ObterPorIdAsync(cmd.CursoId, cmd.Id, false)).ReturnsAsync(new Aula(cmd.CursoId, "A", "d", 1, 10, "u", "V", true, ""));
        _aulas.Setup(r => r.DespublicarAulaAsync(cmd.CursoId, cmd.Id)).ThrowsAsync(new InvalidOperationException("boom"));

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        res.ObterErros().Any(e => e.Contains("Erro ao despublicar aula: boom")).Should().BeTrue();
        _uow.Verify(u => u.Commit(), Times.Never);
    }
}
