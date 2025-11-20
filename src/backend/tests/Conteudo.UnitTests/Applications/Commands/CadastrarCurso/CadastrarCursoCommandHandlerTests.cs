using Conteudo.Application.Commands.CadastrarCurso;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Data;
using Core.Mediator;
using Core.Messages;

namespace Conteudo.UnitTests.Applications.Commands.CadastrarCurso;
public class CadastrarCursoCommandHandlerTests
{
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<ICursoRepository> _cursos = new();
    private readonly Mock<ICategoriaRepository> _categorias = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CadastrarCursoCommandHandler _sut;

    public CadastrarCursoCommandHandlerTests()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _cursos.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        _sut = new CadastrarCursoCommandHandler(_mediator.Object, _cursos.Object, _categorias.Object);
    }

    private static CadastrarCursoCommand NovoValido() => new CadastrarCursoCommand
    {
        Nome = "Arquitetura Limpa",
        Valor = 499.9m,
        DuracaoHoras = 40,
        Nivel = "Intermediário",
        Instrutor = "Tio Bob",
        VagasMaximas = 30,
        ImagemUrl = "https://cdn.exemplo.com/course.png", // precisa ser URL absoluta pelo validator
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

    [Fact]
    public async Task Deve_retornar_invalido_e_notificar_quando_command_invalido()
    {
        var cmd = new CadastrarCursoCommand
        {
            Nome = "",
            Valor = -1,
            DuracaoHoras = 0,
            Nivel = "",
            Instrutor = "",
            VagasMaximas = 0,
            ImagemUrl = "", // inválida (deve ser URL absoluta)
            Resumo = "",
            Descricao = "",
            Objetivos = ""
        };

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _cursos.Verify(r => r.Adicionar(It.IsAny<Curso>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_nome_duplicado()
    {
        var cmd = NovoValido();
        _cursos.Setup(r => r.ExistePorNomeAsync(cmd.Nome, null)).ReturnsAsync(true);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();     // sem ValidationFailures; usa DomainNotificacao
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_categoria_informada_nao_existir()
    {
        var cmd = NovoValido();
        cmd.CategoriaId = Guid.NewGuid();

        _cursos.Setup(r => r.ExistePorNomeAsync(cmd.Nome, null)).ReturnsAsync(false);
        _categorias.Setup(r => r.ObterPorIdAsync(cmd.CategoriaId.Value, false)).ReturnsAsync((Categoria?)null);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_adicionar_commit_e_retornar_Id()
    {
        var cmd = NovoValido();
        _cursos.Setup(r => r.ExistePorNomeAsync(cmd.Nome, null)).ReturnsAsync(false);

        Curso? criado = null;
        _cursos.Setup(r => r.Adicionar(It.IsAny<Curso>()))
               .Callback<Curso>(c => criado = c);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        criado.Should().NotBeNull();
        criado!.Nome.Should().Be(cmd.Nome);
        _uow.Verify(u => u.Commit(), Times.Once);

        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(criado.Id);
    }
}
