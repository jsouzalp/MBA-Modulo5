using Conteudo.Application.Commands.CadastrarAula;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Data;
using Core.Mediator;
using Core.Messages;

namespace Conteudo.UnitTests.Applications.Commands.CadastrarAula;
public class CadastrarAulaCommandHandlerTests
{
    private readonly Mock<IAulaRepository> _aulas = new();
    private readonly Mock<ICursoRepository> _cursos = new();
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private CadastrarAulaCommandHandler CriarSut()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _aulas.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        return new CadastrarAulaCommandHandler(_aulas.Object, _cursos.Object, _mediator.Object);
    }

    [Fact]
    public async Task Deve_retornar_invalido_quando_command_invalido()
    {
        var cmd = new CadastrarAulaCommand(Guid.Empty, "", "", 0, 0, "", "", true, "");
        var sut = CriarSut();

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _aulas.Verify(r => r.CadastrarAulaAsync(It.IsAny<Aula>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_curso_nao_existir()
    {
        var cmd = new CadastrarAulaCommand(Guid.NewGuid(), "A1", "d", 1, 10, "u", "Vídeo", true, "");
        var sut = CriarSut();

        _cursos.Setup(r => r.ObterPorIdAsync(cmd.CursoId, false, true)).ReturnsAsync((Curso?)null);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_numero_ja_existir_no_curso()
    {
        var cmd = new CadastrarAulaCommand(Guid.NewGuid(), "A1", "d", 7, 10, "u", "Vídeo", true, "");
        var sut = CriarSut();

        _cursos.Setup(r => r.ObterPorIdAsync(cmd.CursoId, false, true)).ReturnsAsync(new Curso("C", 10, new("r", "d", "o", "pr", "pa", "m", "r", "a", "b"), 1, "N", "I", 1, "", null, null));
        _aulas.Setup(r => r.ExistePorNumeroAsync(cmd.CursoId, cmd.Numero, null)).ReturnsAsync(true);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _aulas.Verify(r => r.CadastrarAulaAsync(It.IsAny<Aula>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_criar_chamar_repo_e_commit_e_retornar_Id()
    {
        var cmd = new CadastrarAulaCommand(Guid.NewGuid(), "Intro", "d", 1, 30, "url", "Vídeo", true, "");
        var sut = CriarSut();

        _cursos.Setup(r => r.ObterPorIdAsync(cmd.CursoId, false, true)).ReturnsAsync(new Curso("C", 10, new("r", "d", "o", "pr", "pa", "m", "r", "a", "b"), 1, "N", "I", 1, "", null, null));
        _aulas.Setup(r => r.ExistePorNumeroAsync(cmd.CursoId, cmd.Numero, null)).ReturnsAsync(false);

        Aula? criada = null;
        _aulas.Setup(r => r.CadastrarAulaAsync(It.IsAny<Aula>()))
              .Callback<Aula>(a => criada = a)
              .Returns((Aula a) => Task.FromResult(a));

        var res = await sut.Handle(cmd, CancellationToken.None);

        criada.Should().NotBeNull();
        criada!.Nome.Should().Be("Intro");
        res.IsValid.Should().BeTrue();
        res.Data.Should().BeOfType<Guid>().And.Be(criada.Id);
        _uow.Verify(u => u.Commit(), Times.Once);
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Never);
    }
}
