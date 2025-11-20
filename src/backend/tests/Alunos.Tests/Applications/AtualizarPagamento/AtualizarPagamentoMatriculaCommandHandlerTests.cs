using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Core.Data;
using Core.Mediator;
using FluentAssertions;
using Moq;
using Alunos.Application.Commands.AtualizarPagamento;
using Core.Messages;

namespace Alunos.Tests.Applications.AtualizarPagamento;
public class AtualizarPagamentoMatriculaCommandHandlerTests
{
    private readonly Mock<IAlunoRepository> _alunos = new();
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private AtualizarPagamentoMatriculaCommandHandler CriarSut()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _alunos.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        return new AtualizarPagamentoMatriculaCommandHandler(_alunos.Object, _mediator.Object);
    }

    private static Aluno NovoAluno()
    {
        var aluno = new Aluno(Guid.NewGuid(), "Fulano", "f@e.com", "12345678909", new DateTime(1990, 1, 1), "M", "SP", "SP", "01001000", "foto");
        aluno.AtivarAluno();
        aluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Curso de DDD", 500m, null);

        return aluno;
    }

    [Fact]
    public async Task Deve_retornar_invalido_quando_command_invalido()
    {
        var cmd = new AtualizarPagamentoMatriculaCommand(Guid.Empty, Guid.Empty); // inválido pelo validator
        var sut = CriarSut();

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _alunos.Verify(r => r.AtualizarAsync(It.IsAny<Aluno>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_aluno_nao_for_encontrado()
    {
        var cmd = new AtualizarPagamentoMatriculaCommand(Guid.NewGuid(), Guid.NewGuid());
        var sut = CriarSut();

        _alunos.Setup(r => r.ObterPorIdAsync(cmd.AlunoId, true)).ReturnsAsync((Aluno?)null);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();       // handler notifica mas não adiciona failures
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_atualizar_commit_e_retornar_true()
    {
        var aluno = NovoAluno();
        var matricula = aluno.MatriculasCursos.FirstOrDefault();

        var cmd = new AtualizarPagamentoMatriculaCommand(aluno.Id, matricula?.Id ?? Guid.Empty);

        var sut = CriarSut();
        _alunos.Setup(r => r.ObterPorIdAsync(cmd.AlunoId, true)).ReturnsAsync(aluno);
        _alunos.Setup(r => r.AtualizarAsync(aluno)).Returns(Task.CompletedTask);

        var res = await sut.Handle(cmd, CancellationToken.None);

        _alunos.Verify(r => r.AtualizarAsync(aluno), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Once);
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Never);

        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(true);
    }

    [Fact]
    public async Task Commit_false_deixa_Data_nula()
    {
        var aluno = NovoAluno();
        var matricula = aluno.MatriculasCursos.FirstOrDefault();

        var cmd = new AtualizarPagamentoMatriculaCommand(aluno.Id, matricula?.Id ?? Guid.Empty);
        var sut = CriarSut();

        _alunos.Setup(r => r.ObterPorIdAsync(cmd.AlunoId, true)).ReturnsAsync(aluno);
        _uow.Setup(u => u.Commit()).ReturnsAsync(false);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull(); // handler só seta Data quando commit==true
    }
}
