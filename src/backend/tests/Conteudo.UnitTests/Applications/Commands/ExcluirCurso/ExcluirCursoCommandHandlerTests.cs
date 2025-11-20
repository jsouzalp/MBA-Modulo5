using Conteudo.Application.Commands.ExcluirCurso;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Domain.ValueObjects;
using Core.Data;
using Core.Mediator;
using Core.Messages;

namespace Conteudo.UnitTests.Applications.Commands.ExcluirCurso;
public class ExcluirCursoCommandHandlerTests
{
    private readonly Mock<ICursoRepository> _cursos = new();
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly ExcluirCursoCommandHandler _sut;

    public ExcluirCursoCommandHandlerTests()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _cursos.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        _sut = new ExcluirCursoCommandHandler(_cursos.Object, _mediator.Object);
    }

    private static Curso CursoVazio()
        => new("C", 10, new ConteudoProgramatico("r", "d", "o", "pr", "pa", "m", "r", "a", "b"), 1, "N", "I", 1, "", null, null);

    [Fact]
    public async Task Deve_retornar_invalido_quando_command_invalido()
    {
        var res = await _sut.Handle(new ExcluirCursoCommand(Guid.Empty), CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _cursos.Verify(r => r.Deletar(It.IsAny<Curso>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_curso_nao_encontrado()
    {
        var cmd = new ExcluirCursoCommand(Guid.NewGuid());
        _cursos.Setup(r => r.ObterPorIdAsync(cmd.Id, true, false)).ReturnsAsync((Curso?)null);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_deletar_commit_e_retornar_true()
    {
        var curso = CursoVazio(); // sem aulas

        var cmd = new ExcluirCursoCommand(curso.Id);
        _cursos.Setup(r => r.ObterPorIdAsync(cmd.Id, false, true)).ReturnsAsync(curso);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        _cursos.Verify(r => r.Deletar(curso), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Once);

        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(true);
    }

    [Fact]
    public async Task Commit_false_deve_notificar_e_manter_Data_nula()
    {
        var cmd = new ExcluirCursoCommand(Guid.NewGuid());
        var curso = CursoVazio();
        _cursos.Setup(r => r.ObterPorIdAsync(cmd.Id, false, true)).ReturnsAsync(curso);
        _uow.Setup(u => u.Commit()).ReturnsAsync(false);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Once);
    }
}
