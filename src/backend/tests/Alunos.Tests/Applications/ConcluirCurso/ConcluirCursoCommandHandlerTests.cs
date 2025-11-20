using Alunos.Application.Commands.ConcluirCurso;
using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Core.Data;
using Core.Mediator;
using Core.SharedDtos.Conteudo;
using FluentAssertions;
using Moq;
using Core.Messages;

namespace Alunos.Tests.Applications.ConcluirCurso;
public class ConcluirCursoCommandHandlerTests
{
    private readonly Mock<IAlunoRepository> _repo = new();
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private ConcluirCursoCommandHandler CriarSut()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _repo.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        return new ConcluirCursoCommandHandler(_repo.Object, _mediator.Object);
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
    public async Task Deve_retornar_invalido_quando_command_invalido()
    {
        var cmd = new ConcluirCursoCommand(Guid.Empty, Guid.Empty, cursoDto: null!);
        var sut = CriarSut();

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _repo.Verify(r => r.AtualizarAsync(It.IsAny<Aluno>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_aluno_nao_encontrado()
    {
        var cmd = new ConcluirCursoCommand(Guid.NewGuid(), Guid.NewGuid(), new CursoDto { Id = Guid.NewGuid(), Aulas = [] });
        var sut = CriarSut();

        _repo.Setup(r => r.ObterPorIdAsync(cmd.AlunoId, true)).ReturnsAsync((Aluno?)null);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_existirem_aulas_nao_iniciadas()
    {
        var aluno = NovoAluno();
        var matricula = aluno.MatriculasCursos.FirstOrDefault();

        // CursoDto com 2 aulas ativas e aluno com 0 registros -> dispara regra "não iniciadas"
        var cmd = new ConcluirCursoCommand(aluno.Id, matricula?.Id ?? Guid.Empty, new CursoDto { Id = Guid.NewGuid(), Aulas = [new(), new()] });
        var sut = CriarSut();

        _repo.Setup(r => r.ObterPorIdAsync(cmd.AlunoId, true)).ReturnsAsync(aluno);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_quando_sem_pendencias_e_sem_nao_iniciadas()
    {
        var aluno = NovoAluno();
        var matricula = aluno.MatriculasCursos.FirstOrDefault();

        // CursoDto sem aulas → passa nas duas verificações
        var cmd = new ConcluirCursoCommand(aluno.Id, matricula?.Id ?? Guid.Empty, new CursoDto { Id = Guid.NewGuid(), Aulas = [] });
        var sut = CriarSut();

        _repo.Setup(r => r.ObterPorIdAsync(cmd.AlunoId, true)).ReturnsAsync(aluno);

        var res = await sut.Handle(cmd, CancellationToken.None);

        _repo.Verify(r => r.AtualizarAsync(It.IsAny<Aluno>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Once);

        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(true);
    }

    [Fact]
    public async Task Commit_false_deixa_Data_nula()
    {
        var aluno = NovoAluno();
        var matricula = aluno.MatriculasCursos.FirstOrDefault();

        var cmd = new ConcluirCursoCommand(aluno.Id, matricula?.Id ?? Guid.Empty, new CursoDto { Id = Guid.NewGuid(), Aulas = [] });
        var sut = CriarSut();

        _repo.Setup(r => r.ObterPorIdAsync(cmd.AlunoId, true)).ReturnsAsync(aluno);
        _uow.Setup(u => u.Commit()).ReturnsAsync(false);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
    }
}
