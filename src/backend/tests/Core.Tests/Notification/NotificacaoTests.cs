using Core.Notification;

namespace Core.Tests.Notification;

public class NotificacaoTests 
{
    [Fact]
    public void Notificacao_DeveCriarComPropriedadesPadrao()
    {
        // Arrange & Act
        var notificacao = new Notificacao();

        // Assert
        notificacao.Should().NotBeNull();
        notificacao.Tipo.Should().Be(TipoNotificacao.Informacao);
        notificacao.Mensagem.Should().Be(string.Empty);
    }

    [Fact]
    public void Notificacao_DevePermitirDefinirTipo()
    {
        // Arrange
        var notificacao = new Notificacao();
        var tipo = TipoNotificacao.Erro;

        // Act
        notificacao.Tipo = tipo;

        // Assert
        notificacao.Tipo.Should().Be(tipo);
    }

    [Fact]
    public void Notificacao_DevePermitirDefinirMensagem()
    {
        // Arrange
        var notificacao = new Notificacao();
        var mensagem = "Mensagem de teste";

        // Act
        notificacao.Mensagem = mensagem;

        // Assert
        notificacao.Mensagem.Should().Be(mensagem);
    }

    [Fact]
    public void Notificacao_DevePermitirMensagemVazia()
    {
        // Arrange
        var notificacao = new Notificacao();

        // Act
        notificacao.Mensagem = string.Empty;

        // Assert
        notificacao.Mensagem.Should().Be(string.Empty);
    }

    [Fact]
    public void Notificacao_DevePermitirMensagemNull()
    {
        // Arrange
        var notificacao = new Notificacao();

        // Act
        notificacao.Mensagem = null!;

        // Assert
        notificacao.Mensagem.Should().BeNull();
    }

    [Fact]
    public void Notificacao_DevePermitirTodosOsTipos()
    {
        // Arrange
        var notificacao = new Notificacao();

        // Act & Assert
        notificacao.Tipo = TipoNotificacao.Informacao;
        notificacao.Tipo.Should().Be(TipoNotificacao.Informacao);

        notificacao.Tipo = TipoNotificacao.Erro;
        notificacao.Tipo.Should().Be(TipoNotificacao.Erro);
    }

    [Fact]
    public void Notificacao_DeveManterPropriedadesIndependentes()
    {
        // Arrange
        var notificacao1 = new Notificacao();
        var notificacao2 = new Notificacao();

        // Act
        notificacao1.Tipo = TipoNotificacao.Erro;
        notificacao1.Mensagem = "Erro 1";
        notificacao2.Tipo = TipoNotificacao.Informacao;
        notificacao2.Mensagem = "Info 2";

        // Assert
        notificacao1.Tipo.Should().Be(TipoNotificacao.Erro);
        notificacao1.Mensagem.Should().Be("Erro 1");
        notificacao2.Tipo.Should().Be(TipoNotificacao.Informacao);
        notificacao2.Mensagem.Should().Be("Info 2");
    }

    [Fact]
    public void Defaults_e_setters()
    {
        var n = new Notificacao();

        // defaults
        n.Tipo.Should().Be(TipoNotificacao.Informacao); // enum default = 0
        n.Mensagem.Should().Be(string.Empty);

        // setters
        n.Tipo = TipoNotificacao.Erro;
        n.Mensagem = "falhou";

        n.Tipo.Should().Be(TipoNotificacao.Erro);
        n.Mensagem.Should().Be("falhou");
    }
}
