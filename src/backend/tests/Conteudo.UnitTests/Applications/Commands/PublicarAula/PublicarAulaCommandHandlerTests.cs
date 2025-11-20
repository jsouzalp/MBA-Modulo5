
using Conteudo.Application.Commands.PublicarAula;
using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Data;
using Core.Mediator;
using Core.Messages;

namespace Conteudo.UnitTests.Applications.Commands.PublicarAula;

public class PublicarAulaCommandHandlerTests
{
    private readonly Mock<IAulaRepository> _aulaRepositoryMock;
    private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly PublicarAulaCommandHandler _handler;

    public PublicarAulaCommandHandlerTests()
    {
        _aulaRepositoryMock = new Mock<IAulaRepository>();
        _mediatorHandlerMock = new Mock<IMediatorHandler>();
        _uowMock = new Mock<IUnitOfWork>();

        _uowMock.Setup(u => u.Commit()).ReturnsAsync(true);
        _aulaRepositoryMock.Setup(r => r.UnitOfWork).Returns(_uowMock.Object);

        _handler = new PublicarAulaCommandHandler(
            _aulaRepositoryMock.Object,
            _mediatorHandlerMock.Object
        );
    }

    private static Aula AulaValida(Guid cursoId) =>
        new Aula(cursoId, "Aula 1", "desc", 1, 30, "https://v", "Vídeo", true, "");

    [Fact]
    public async Task Deve_retornar_invalido_quando_command_invalido()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        var cmd = new PublicarAulaCommand(cursoId, Guid.Empty); // inválido pelo validator

        // Act
        var resultado = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        resultado.IsValid.Should().BeFalse();
        _mediatorHandlerMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _aulaRepositoryMock.Verify(r => r.PublicarAulaAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _uowMock.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_aula_nao_for_encontrada()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();
        var cmd = new PublicarAulaCommand(cursoId, aulaId);

        _aulaRepositoryMock
            .Setup(r => r.ObterPorIdAsync(cursoId, aulaId, false))
            .ReturnsAsync((Aula?)null);

        // Act
        var resultado = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        _mediatorHandlerMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _aulaRepositoryMock.Verify(r => r.PublicarAulaAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _uowMock.Verify(u => u.Commit(), Times.Never);

        // Pela implementação atual, o handler apenas notifica e não adiciona falhas no ValidationResult.
        resultado.IsValid.Should().BeTrue();
        resultado.Data.Should().BeNull();
    }

    [Fact]
    public async Task Deve_publicar_e_commit_quando_aula_existir()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();
        var cmd = new PublicarAulaCommand(cursoId, aulaId);

        _aulaRepositoryMock
            .Setup(r => r.ObterPorIdAsync(cursoId, aulaId, false))
            .ReturnsAsync(AulaValida(cursoId));

        // Act
        var resultado = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        _aulaRepositoryMock.Verify(r => r.PublicarAulaAsync(cursoId, aulaId), Times.Once);
        _uowMock.Verify(u => u.Commit(), Times.Once);
        _mediatorHandlerMock.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Never);

        resultado.IsValid.Should().BeTrue();
        resultado.Data.Should().Be(aulaId);
    }

    [Fact]
    public async Task Deve_retornar_invalido_e_nao_commit_quando_repo_lancar_excecao()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        var aulaId = Guid.NewGuid();
        var cmd = new PublicarAulaCommand(cursoId, aulaId);

        _aulaRepositoryMock
            .Setup(r => r.ObterPorIdAsync(cursoId, aulaId, false))
            .ReturnsAsync(AulaValida(cursoId));

        _aulaRepositoryMock
            .Setup(r => r.PublicarAulaAsync(cursoId, aulaId))
            .ThrowsAsync(new InvalidOperationException("boom"));

        // Act
        var resultado = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.ObterErros().Any(e => e.Contains("Erro ao publicar aula")).Should().BeTrue();
        _uowMock.Verify(u => u.Commit(), Times.Never);
    }
}
