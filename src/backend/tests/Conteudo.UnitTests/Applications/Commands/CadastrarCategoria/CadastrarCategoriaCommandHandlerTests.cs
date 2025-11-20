using Conteudo.Application.Commands.CadastrarCategoria;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Data;
using Core.Mediator;
using Core.Messages;

namespace Conteudo.UnitTests.Applications.Commands.CadastrarCategoria;
public class CadastrarCategoriaCommandHandlerTests
{
    private readonly Mock<ICategoriaRepository> _categorias = new();
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CadastrarCategoriaCommandHandler _sut;

    public CadastrarCategoriaCommandHandlerTests()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _categorias.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        _sut = new CadastrarCategoriaCommandHandler(_categorias.Object, _mediator.Object);
    }

    [Fact]
    public async Task Deve_retornar_invalido_quando_command_invalido()
    {
        var cmd = new CadastrarCategoriaCommand
        {
            Nome = "",
            Descricao = "",
            Cor = "xyz",
            IconeUrl = new string('x', 600)
        };

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _categorias.Verify(r => r.Adicionar(It.IsAny<Categoria>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_nome_duplicado()
    {
        var cmd = new CadastrarCategoriaCommand { Nome = "Arq", Descricao = "d", Cor = "#123", IconeUrl = "" };

        _categorias.Setup(r => r.ExistePorNome(cmd.Nome)).ReturnsAsync(true);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_adicionar_e_commit_e_retornar_Id()
    {
        var cmd = new CadastrarCategoriaCommand { Nome = "Engenharia", Descricao = "d", Cor = "#123", IconeUrl = "", Ordem = 3 };

        Categoria? criada = null;
        _categorias.Setup(r => r.ExistePorNome(cmd.Nome)).ReturnsAsync(false);
        _categorias.Setup(r => r.Adicionar(It.IsAny<Categoria>()))
                   .Callback<Categoria>(c => criada = c);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        criada.Should().NotBeNull();
        criada!.Nome.Should().Be("Engenharia");

        _uow.Verify(u => u.Commit(), Times.Once);
        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(criada.Id);
    }
}
