using Alunos.Application.Commands.MatricularAluno;
using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Core.Data;
using Core.Mediator;
using FluentAssertions;
using Moq;
using Core.Messages;

namespace Alunos.Tests.Applications.MatricularAluno;
public class MatricularAlunoCommandHandlerTests
{
    private readonly Mock<IAlunoRepository> _repo = new();
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private MatricularAlunoCommandHandler CriarSut()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _repo.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        return new MatricularAlunoCommandHandler(_repo.Object, _mediator.Object);
    }

    private static Aluno NovoAluno()
    {
        var aluno = new Aluno(Guid.NewGuid(), "Fulano", "f@e.com", "12345678909", new DateTime(1990, 1, 1), "M", "SP", "SP", "01001000", "foto");
        aluno.AtivarAluno();
        aluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Curso de DDD", 500m, null);

        MatriculaCurso matriculaCurso = aluno.MatriculasCursos.First();
        matriculaCurso.RegistrarPagamentoMatricula();

        return aluno;
    }

    [Fact]
    public async Task Deve_retornar_invalido_e_notificar_quando_command_invalido()
    {
        var cmd = new MatricularAlunoCommand(Guid.Empty, Guid.Empty, false, "", 0m, "");
        var sut = CriarSut();

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _repo.Verify(r => r.AdicionarMatriculaCursoAsync(It.IsAny<MatriculaCurso>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_aluno_nao_encontrado()
    {
        var cmd = new MatricularAlunoCommand(Guid.NewGuid(), Guid.NewGuid(), true, "DDD", 100, "ok");
        var sut = CriarSut();

        _repo.Setup(r => r.ObterPorIdAsync(cmd.AlunoId, true)).ReturnsAsync((Aluno?)null);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_matricular_adicionar_commit_e_retornar_Id_da_matricula()
    {
        var aluno = NovoAluno();

        var cmd = new MatricularAlunoCommand(aluno.Id, Guid.NewGuid(), true, "Curso avançado de DDD", 100, "obs");
        var sut = CriarSut();
        _repo.Setup(r => r.ObterPorIdAsync(cmd.AlunoId, true)).ReturnsAsync(aluno);

        MatriculaCurso? criada = null;
        _repo.Setup(r => r.AdicionarMatriculaCursoAsync(It.IsAny<MatriculaCurso>()))
             .Callback<MatriculaCurso>(m => criada = m)
             .Returns(Task.CompletedTask);

        var res = await sut.Handle(cmd, CancellationToken.None);

        criada.Should().NotBeNull();
        _uow.Verify(u => u.Commit(), Times.Once);
        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(criada!.Id);
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Never);
    }

    [Fact]
    public async Task Commit_false_deixa_Data_nula()
    {
        var aluno = NovoAluno();

        var cmd = new MatricularAlunoCommand(aluno.Id, Guid.NewGuid(), true, "Curso avançado de DDD", 100, "obs");
        var sut = CriarSut();
        _repo.Setup(r => r.ObterPorIdAsync(cmd.AlunoId, true)).ReturnsAsync(aluno);
        _uow.Setup(u => u.Commit()).ReturnsAsync(false);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
    }
}
