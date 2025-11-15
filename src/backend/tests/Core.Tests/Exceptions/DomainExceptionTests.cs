using Plataforma.Educacao.Core.Exceptions;

namespace Core.Tests.Exceptions;

public class DomainExceptionTests
{
    [Fact]
    public void DomainException_DeveCriarComMensagem()
    {
        // Arrange
        var mensagem = "Erro de domínio";

        // Act
        var exception = new DomainException(mensagem);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be(mensagem);
        exception.Errors.Should().HaveCount(1);
        exception.Errors.Should().Contain(mensagem);
    }

    [Fact]
    public void DomainException_DeveCriarComColecaoDeMensagens()
    {
        // Arrange
        var mensagens = new[] { "Erro 1", "Erro 2", "Erro 3" };

        // Act
        var exception = new DomainException(mensagens);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be("Erro 1; Erro 2; Erro 3");
        exception.Errors.Should().HaveCount(3);
        exception.Errors.Should().Contain("Erro 1");
        exception.Errors.Should().Contain("Erro 2");
        exception.Errors.Should().Contain("Erro 3");
    }

    [Fact]
    public void DomainException_DeveCriarComListaDeMensagens()
    {
        // Arrange
        var mensagens = new List<string> { "Erro A", "Erro B" };

        // Act
        var exception = new DomainException(mensagens);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be("Erro A; Erro B");
        exception.Errors.Should().HaveCount(2);
        exception.Errors.Should().Contain("Erro A");
        exception.Errors.Should().Contain("Erro B");
    }

    [Fact]
    public void DomainException_DeveCriarComMensagemVazia()
    {
        // Arrange
        var mensagem = string.Empty;

        // Act
        var exception = new DomainException(mensagem);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be(mensagem);
        exception.Errors.Should().HaveCount(1);
        exception.Errors.Should().Contain(mensagem);
    }

    [Fact]
    public void DomainException_DeveCriarComColecaoVazia()
    {
        // Arrange
        var mensagens = new string[0];

        // Act
        var exception = new DomainException(mensagens);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be(string.Empty);
        exception.Errors.Should().BeEmpty();
    }

    [Fact]
    public void DomainException_DeveCriarComColecaoNull()
    {
        // Arrange
        string[]? mensagens = null;

        // Act & Assert
        var action = () => new DomainException(mensagens!);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void DomainException_DeveHerdarDeException()
    {
        // Arrange & Act
        var exception = new DomainException("Teste");

        // Assert
        exception.Should().BeAssignableTo<Exception>();
    }

    [Fact]
    public void DomainException_DeveTerErrorsComoReadOnly()
    {
        // Arrange
        var exception = new DomainException("Teste");

        // Act & Assert
        exception.Errors.Should().NotBeNull();
    }

    [Fact]
    public void SingleMessage_deve_preencher_Message_e_Errors()
    {
        var ex = new DomainException("erro X");

        ex.Message.Should().Contain("erro X");
        ex.Errors.Should().ContainSingle().Which.Should().Be("erro X");
    }

    [Fact]
    public void MultipleMessages_deve_concatenar_Message_e_preservar_lista()
    {
        var ex = new DomainException(new[] { "e1", "e2" });

        ex.Message.Should().Contain("e1").And.Contain("e2");
        ex.Errors.Should().BeEquivalentTo(new[] { "e1", "e2" });
    }
}
