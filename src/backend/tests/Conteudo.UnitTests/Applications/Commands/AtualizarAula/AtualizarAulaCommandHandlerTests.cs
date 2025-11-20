using Conteudo.Application.Commands.AtualizarAula;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Data;
using Core.Mediator;
using Core.Messages;

namespace Conteudo.UnitTests.Applications.Commands.AtualizarAula;
public class AtualizarAulaCommandHandlerTests
{
    private readonly Mock<IAulaRepository> _aulas = new();
    private readonly Mock<ICursoRepository> _cursos = new();
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly AtualizarAulaCommandHandler _sut;

    public AtualizarAulaCommandHandlerTests()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _aulas.Setup(r => r.UnitOfWork).Returns(_uow.Object);

        _sut = new AtualizarAulaCommandHandler(_aulas.Object, _cursos.Object, _mediator.Object);
    }

    private static Aula AulaValida(Guid cursoId)
        => new(cursoId, "A1", "desc", 1, 30, "https://v", "Vídeo", true, "");

    [Fact]
    public async Task Deve_retornar_invalido_quando_command_invalido()
    {
        var cmd = new AtualizarAulaCommand(
            id: Guid.Empty,                    // inválido
            cursoId: Guid.NewGuid(),
            nome: "",                          // inválido
            descricao: "d",
            numero: 0,                         // inválido
            duracaoMinutos: -1,                // inválido
            videoUrl: "",
            tipoAula: "",
            isObrigatoria: true,
            observacoes: ""
        );

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _aulas.Verify(r => r.AtualizarAulaAsync(It.IsAny<Aula>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_aula_nao_encontrada()
    {
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();
        var cmd = new AtualizarAulaCommand(aulaId, cursoId, "A1", "d", 1, 10, "u", "Vídeo", true, "");

        _aulas.Setup(r => r.ObterPorIdAsync(cursoId, aulaId, false))
              .ReturnsAsync((Aula?)null);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_curso_nao_encontrado()
    {
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();
        var cmd = new AtualizarAulaCommand(aulaId, cursoId, "A1", "d", 1, 10, "u", "Vídeo", true, "");

        _aulas.Setup(r => r.ObterPorIdAsync(cursoId, aulaId, false))
              .ReturnsAsync(AulaValida(cursoId));
        _cursos.Setup(r => r.ObterPorIdAsync(cursoId, false, true))
               .ReturnsAsync((Conteudo.Domain.Entities.Curso?)null);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_aula_nao_pertence_ao_curso()
    {
        var cursoSolicitado = Guid.NewGuid();
        var outroCurso = Guid.NewGuid();
        var aulaId = Guid.NewGuid();
        var cmd = new AtualizarAulaCommand(aulaId, cursoSolicitado, "A1", "d", 1, 10, "u", "Vídeo", true, "");

        _aulas.Setup(r => r.ObterPorIdAsync(cursoSolicitado, aulaId, false))
              .ReturnsAsync(AulaValida(outroCurso));
        _cursos.Setup(r => r.ObterPorIdAsync(cursoSolicitado, false, true))
               .ReturnsAsync(new Conteudo.Domain.Entities.Curso("C", 10, new("r", "d", "o", "pr", "pa", "m", "r", "a", "b"), 1, "N", "I", 1, "", null, null));

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_numero_duplicado()
    {
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();
        var cmd = new AtualizarAulaCommand(aulaId, cursoId, "A1", "d", 10, 10, "u", "Vídeo", true, "");

        _aulas.Setup(r => r.ObterPorIdAsync(cursoId, aulaId, false))
              .ReturnsAsync(AulaValida(cursoId));
        _cursos.Setup(r => r.ObterPorIdAsync(cursoId, false, true))
               .ReturnsAsync(new Conteudo.Domain.Entities.Curso("C", 10, new("r", "d", "o", "pr", "pa", "m", "r", "a", "b"), 1, "N", "I", 1, "", null, null));
        _aulas.Setup(r => r.ExistePorNumeroAsync(cursoId, 10, aulaId)).ReturnsAsync(true);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_atualizar_chamar_repo_e_commit_e_retornar_true_em_Data()
    {
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();
        var cmd = new AtualizarAulaCommand(aulaId, cursoId, "Novo Nome", "Nova desc", 2, 50, "https://novo", "Vídeo", false, "obs");

        var aula = AulaValida(cursoId);
        _aulas.Setup(r => r.ObterPorIdAsync(cursoId, aulaId, false)).ReturnsAsync(aula);
        _cursos.Setup(r => r.ObterPorIdAsync(cursoId, false, true))
               .ReturnsAsync(new Conteudo.Domain.Entities.Curso("C", 10, new("r", "d", "o", "pr", "pa", "m", "r", "a", "b"), 1, "N", "I", 1, "", null, null));
        _aulas.Setup(r => r.ExistePorNumeroAsync(cursoId, 2, aulaId)).ReturnsAsync(false);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        _aulas.Verify(r => r.AtualizarAulaAsync(It.Is<Aula>(a =>
            a.Nome == "Novo Nome" &&
            a.Numero == 2 &&
            a.DuracaoMinutos == 50 &&
            a.VideoUrl == "https://novo" &&
            a.IsObrigatoria == false)), Times.Once);

        _uow.Verify(u => u.Commit(), Times.Once);
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Never);

        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(true);
    }
}
