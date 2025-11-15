using Conteudo.Application.Commands.CadastrarMaterial;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Data;
using Core.Mediator;
using Core.Messages;

namespace Conteudo.UnitTests.Applications.Commands.CadastrarMaterial;
public class CadastrarMaterialCommandHandlerTests
{
    private readonly Mock<IMaterialRepository> _materiais = new();
    private readonly Mock<IAulaRepository> _aulas = new();
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private CadastrarMaterialCommandHandler CriarSut()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _materiais.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        return new CadastrarMaterialCommandHandler(_materiais.Object, _aulas.Object, _mediator.Object);
    }

    [Fact]
    public async Task Deve_retornar_invalido_e_notificar_quando_command_invalido()
    {
        var cmd = new CadastrarMaterialCommand(Guid.NewGuid(), Guid.Empty, "", "", "", "", true, -1, ".pdf", -1);
        var sut = CriarSut();

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _materiais.Verify(r => r.CadastrarMaterialAsync(It.IsAny<Material>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_aula_nao_existir()
    {
        var cmd = new CadastrarMaterialCommand(Guid.NewGuid(), Guid.NewGuid(), "M1", "d", "PDF", "u", false, 0, ".pdf", 0);
        var sut = CriarSut();

        _aulas.Setup(r => r.ObterPorIdAsync(cmd.CursoId, cmd.AulaId, true)).ReturnsAsync((Aula?)null);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_nome_duplicado_na_mesma_aula()
    {
        var cmd = new CadastrarMaterialCommand(Guid.NewGuid(), Guid.NewGuid(), "M1", "d", "PDF", "u", false, 0, ".pdf", 0);
        var sut = CriarSut();

        _aulas.Setup(r => r.ObterPorIdAsync(cmd.CursoId, cmd.AulaId, true)).ReturnsAsync(new Aula(cmd.CursoId, "A1", "d", 1, 10, "u", "V", true, ""));
        _materiais.Setup(r => r.ExistePorNomeAsync(cmd.AulaId, cmd.Nome, null)).ReturnsAsync(true);

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _materiais.Verify(r => r.CadastrarMaterialAsync(It.IsAny<Material>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_cadastrar_e_commit_e_retornar_Id()
    {
        var cmd = new CadastrarMaterialCommand(Guid.NewGuid(), Guid.NewGuid(), "M1", "d", "PDF", "u", false, 0, ".pdf", 0);
        var sut = CriarSut();

        _aulas.Setup(r => r.ObterPorIdAsync(cmd.CursoId, cmd.AulaId, false)).ReturnsAsync(new Aula(cmd.CursoId, "A1", "d", 1, 10, "u", "V", true, ""));
        _materiais.Setup(r => r.ExistePorNomeAsync(cmd.AulaId, cmd.Nome, null)).ReturnsAsync(false);

        Material? criado = null;
        _materiais.Setup(r => r.CadastrarMaterialAsync(It.IsAny<Material>()))
                  .Callback<Material>(m => criado = m)
                  .Returns((Material m) => Task.FromResult(m));

        var res = await sut.Handle(cmd, CancellationToken.None);

        criado.Should().NotBeNull();
        criado!.Nome.Should().Be("M1");
        _uow.Verify(u => u.Commit(), Times.Once);

        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(criado.Id);
    }

    [Fact]
    public async Task Excecao_deve_retornar_invalido_e_nao_commit()
    {
        var cmd = new CadastrarMaterialCommand(Guid.NewGuid(), Guid.NewGuid(), "M1", "d", "PDF", "u", false, 0, ".pdf", 0);
        var sut = CriarSut();

        _aulas.Setup(r => r.ObterPorIdAsync(cmd.CursoId, cmd.AulaId, false)).ReturnsAsync(new Aula(cmd.CursoId, "A1", "d", 1, 10, "u", "V", true, ""));
        _materiais.Setup(r => r.ExistePorNomeAsync(cmd.AulaId, cmd.Nome, null)).ReturnsAsync(false);
        _materiais.Setup(r => r.CadastrarMaterialAsync(It.IsAny<Material>()))
                  .ThrowsAsync(new InvalidOperationException("boom"));

        var res = await sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        res.ObterErros().Any(e => e.Contains("Erro ao cadastrar material: boom")).Should().BeTrue();
        _uow.Verify(u => u.Commit(), Times.Never);
    }
}
