using Conteudo.Application.Commands.AtualizarCategoria;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Data;
using Core.Mediator;
using Core.Messages;

namespace Conteudo.UnitTests.Applications.Commands.AtualizarCategoria;
public class AtualizarCategoriaCommandHandlerTests
{
    private readonly Mock<ICategoriaRepository> _categorias = new();
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly AtualizarCategoriaCommandHandler _sut;

    public AtualizarCategoriaCommandHandlerTests()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _categorias.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        _sut = new AtualizarCategoriaCommandHandler(_categorias.Object, _mediator.Object);
    }

    [Fact]
    public async Task Deve_invalidar_quando_command_invalido()
    {
        var cmd = new AtualizarCategoriaCommand
        {
            Id = Guid.Empty, // inválido
            Nome = "",       // inválido
            Descricao = "",
            Cor = "abc"      // inválido (não é hex)
        };

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_categoria_nao_encontrada()
    {
        var id = Guid.NewGuid();
        var cmd = new AtualizarCategoriaCommand
        {
            Id = id,
            Nome = "N",
            Descricao = "D",
            Cor = "#123456"
        };

        _categorias.Setup(r => r.ObterPorIdAsync(id, false)).ReturnsAsync((Categoria?)null);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_nome_duplicado()
    {
        var id = Guid.NewGuid();
        var cmd = new AtualizarCategoriaCommand
        {
            Id = id,
            Nome = "N",
            Descricao = "D",
            Cor = "#123456"
        };

        _categorias.Setup(r => r.ObterPorIdAsync(id, false)).ReturnsAsync(new Categoria("X", "d", "#111111", "", 0));
        _categorias.Setup(r => r.ExistePorNome("N")).ReturnsAsync(true);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_atualizar_e_commit_e_retornar_true()
    {
        var id = Guid.NewGuid();
        var cmd = new AtualizarCategoriaCommand
        {
            Id = id,
            Nome = "Engenharia",
            Descricao = "Nova",
            Cor = "#654321",
            IconeUrl = "i",
            Ordem = 3
        };

        var cat = new Categoria("Arq", "d", "#123456", "", 0);
        _categorias.Setup(r => r.ObterPorIdAsync(id, false)).ReturnsAsync(cat);
        _categorias.Setup(r => r.ExistePorNome("Engenharia")).ReturnsAsync(false);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        _categorias.Verify(r => r.Atualizar(It.Is<Categoria>(c =>
            c.Nome == "Engenharia" && c.Cor == "#654321" && c.Ordem == 3)), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Once);

        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(true);
    }
}
