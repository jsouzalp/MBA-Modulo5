using Conteudo.Application.Commands.AtualizarCurso;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Domain.ValueObjects;
using Core.Data;
using Core.Mediator;
using Core.Messages;

namespace Conteudo.UnitTests.Applications.Commands.AtualizarCurso;
public class AtualizarCursoCommandHandlerTests
{
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<ICursoRepository> _cursos = new();
    private readonly Mock<ICategoriaRepository> _categorias = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly AtualizarCursoCommandHandler _sut;

    public AtualizarCursoCommandHandlerTests()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _cursos.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        _sut = new AtualizarCursoCommandHandler(_mediator.Object, _cursos.Object, _categorias.Object);
    }

    private static Curso NovoCurso() =>
        new("N", 10, new ConteudoProgramatico("r", "d", "o", "pr", "pa", "m", "r", "a", "b"), 1, "Básico", "Instrutor", 10, "", null, null);

    [Fact]
    public async Task Deve_invalidar_quando_command_invalido()
    {
        var cmd = new AtualizarCursoCommand
        {
            Id = Guid.Empty, // inválido
            Nome = "",       // inválido
            Valor = -1,      // inválido
            DuracaoHoras = 0,// inválido
            Nivel = "",
            Instrutor = "",
            VagasMaximas = 0,// inválido
            Resumo = "",
            Descricao = "",
            Objetivos = "" // inválidos
        };

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_curso_nao_encontrado()
    {
        var cmd = new AtualizarCursoCommand
        {
            Id = Guid.NewGuid(),
            Nome = "X",
            Valor = 10,
            DuracaoHoras = 10,
            Nivel = "Básico",
            Instrutor = "I",
            VagasMaximas = 5,
            Resumo = "r",
            Descricao = "d",
            Objetivos = "o"
        };

        _cursos.Setup(r => r.ObterPorIdAsync(cmd.Id, false, true)).ReturnsAsync((Curso?)null);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_nome_duplicado()
    {
        var cmd = new AtualizarCursoCommand
        {
            Id = Guid.NewGuid(),
            Nome = "Duplicado",
            Valor = 10,
            DuracaoHoras = 10,
            Nivel = "Básico",
            Instrutor = "I",
            VagasMaximas = 5,
            Resumo = "r",
            Descricao = "d",
            Objetivos = "o"
        };

        var curso = NovoCurso();
        _cursos.Setup(r => r.ObterPorIdAsync(cmd.Id, false, true)).ReturnsAsync(curso);
        _cursos.Setup(r => r.ExistePorNomeAsync(curso.Nome, curso.Id)).ReturnsAsync(true);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_categoria_configurada_nao_existir()
    {
        var cmd = new AtualizarCursoCommand
        {
            Id = Guid.NewGuid(),
            Nome = "OK",
            Valor = 10,
            DuracaoHoras = 10,
            Nivel = "Básico",
            Instrutor = "I",
            VagasMaximas = 5,
            Resumo = "r",
            Descricao = "d",
            Objetivos = "o"
        };

        var curso = NovoCurso();
        // força categoria definida no "curso carregado" (antes da atualização)
        curso.AtualizarInformacoes(curso.Nome, curso.Valor, curso.ConteudoProgramatico, curso.DuracaoHoras, curso.Nivel, curso.Instrutor, curso.VagasMaximas, "", null, Guid.NewGuid());

        _cursos.Setup(r => r.ObterPorIdAsync(cmd.Id, false, true)).ReturnsAsync(curso);
        _cursos.Setup(r => r.ExistePorNomeAsync(curso.Nome, curso.Id)).ReturnsAsync(false);
        _categorias.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>(), true)).ReturnsAsync((Categoria?)null);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_atualizar_repo_e_commit_e_retornar_true()
    {
        var curso = NovoCurso();

        var cmd = new AtualizarCursoCommand
        {
            Id = curso.Id,
            Nome = "CQRS",
            Valor = 799,
            DuracaoHoras = 60,
            Nivel = "Avançado",
            Instrutor = "G",
            VagasMaximas = 100,
            ImagemUrl = "img",
            ValidoAte = null,
            CategoriaId = null,
            Resumo = "r",
            Descricao = "d",
            Objetivos = "o",
            PreRequisitos = "pr",
            PublicoAlvo = "pa",
            Metodologia = "m",
            Recursos = "r",
            Avaliacao = "a",
            Bibliografia = "b"
        };

        _cursos.Setup(r => r.ObterPorIdAsync(cmd.Id, false, false)).ReturnsAsync(curso);
        _cursos.Setup(r => r.ExistePorNomeAsync(curso.Nome, curso.Id)).ReturnsAsync(false);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        _cursos.Verify(r => r.Atualizar(It.Is<Curso>(c =>
            c.Nome == "CQRS" &&
            c.DuracaoHoras == 60 &&
            c.Nivel == "Avançado" &&
            c.VagasMaximas == 100)), Times.Once);

        _uow.Verify(u => u.Commit(), Times.Once);
        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(true);
    }
}
