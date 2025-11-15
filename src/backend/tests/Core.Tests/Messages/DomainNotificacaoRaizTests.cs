using Core.Messages;

namespace Core.Tests.Messages;

public class DomainNotificacaoRaizTests
{
    [Fact]
    public void DomainNotificacaoRaiz_DeveCriarComRaizAgregacaoEChaveValor()
    {
        // Arrange
        var raizAgregacao = Guid.NewGuid();
        var chave = "Usuario";
        var valor = "Não encontrado";

        // Act
        var notificacao = new DomainNotificacaoRaiz(raizAgregacao, chave, valor);

        // Assert
        notificacao.Should().NotBeNull();
        notificacao.RaizAgregacao.Should().Be(raizAgregacao);
        notificacao.Chave.Should().Be(chave);
        notificacao.Valor.Should().Be(valor);
        notificacao.DataHora.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
        notificacao.NotificacaoId.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void DomainNotificacaoRaiz_DeveCriarComChaveValor()
    {
        // Arrange
        var chave = "Sistema";
        var valor = "Indisponível";

        // Act
        var notificacao = new DomainNotificacaoRaiz(chave, valor);

        // Assert
        notificacao.Should().NotBeNull();
        notificacao.RaizAgregacao.Should().Be(Guid.Empty);
        notificacao.Chave.Should().Be(chave);
        notificacao.Valor.Should().Be(valor);
        notificacao.DataHora.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
        notificacao.NotificacaoId.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void DomainNotificacaoRaiz_DeveGerarIdUnico()
    {
        // Arrange & Act
        var notificacao1 = new DomainNotificacaoRaiz("Chave1", "Valor1");
        var notificacao2 = new DomainNotificacaoRaiz("Chave2", "Valor2");

        // Assert
        notificacao1.NotificacaoId.Should().NotBe(notificacao2.NotificacaoId);
    }

    [Fact]
    public void DomainNotificacaoRaiz_DevePermitirChaveVazia()
    {
        // Arrange
        var chave = string.Empty;
        var valor = "Valor teste";

        // Act
        var notificacao = new DomainNotificacaoRaiz(chave, valor);

        // Assert
        notificacao.Chave.Should().Be(chave);
        notificacao.Valor.Should().Be(valor);
    }

    [Fact]
    public void DomainNotificacaoRaiz_DevePermitirValorVazio()
    {
        // Arrange
        var chave = "Chave teste";
        var valor = string.Empty;

        // Act
        var notificacao = new DomainNotificacaoRaiz(chave, valor);

        // Assert
        notificacao.Chave.Should().Be(chave);
        notificacao.Valor.Should().Be(valor);
    }

    [Fact]
    public void DomainNotificacaoRaiz_DevePermitirChaveENull()
    {
        // Arrange
        string? chave = null;
        var valor = "Valor teste";

        // Act
        var notificacao = new DomainNotificacaoRaiz(chave!, valor);

        // Assert
        notificacao.Chave.Should().BeNull();
        notificacao.Valor.Should().Be(valor);
    }

    [Fact]
    public void DomainNotificacaoRaiz_DevePermitirValorNull()
    {
        // Arrange
        var chave = "Chave teste";
        string? valor = null;

        // Act
        var notificacao = new DomainNotificacaoRaiz(chave, valor!);

        // Assert
        notificacao.Chave.Should().Be(chave);
        notificacao.Valor.Should().BeNull();
    }

    [Fact]
    public void DomainNotificacaoRaiz_DeveManterDataHoraOriginal()
    {
        // Arrange
        var notificacao = new DomainNotificacaoRaiz("Chave", "Valor");
        var dataHoraOriginal = notificacao.DataHora;

        // Act
        Thread.Sleep(100);

        // Assert
        notificacao.DataHora.Should().Be(dataHoraOriginal);
    }

    [Fact]
    public void Ctor_com_raiz_deve_preencher_campos()
    {
        var antes = DateTime.Now.AddSeconds(-1);
        var raiz = Guid.NewGuid();

        var n = new DomainNotificacaoRaiz(raiz, "k", "v");

        n.RaizAgregacao.Should().Be(raiz);
        n.Chave.Should().Be("k");
        n.Valor.Should().Be("v");
        n.NotificacaoId.Should().NotBe(Guid.Empty);
        n.DataHora.Should().BeOnOrAfter(antes);
    }

    [Fact]
    public void Ctor_sem_raiz_deve_gerar_id_e_manter_chave_valor()
    {
        var n = new DomainNotificacaoRaiz("k", "v");

        n.RaizAgregacao.Should().Be(Guid.Empty);
        n.Chave.Should().Be("k");
        n.Valor.Should().Be("v");
        n.NotificacaoId.Should().NotBe(Guid.Empty);
    }
}
