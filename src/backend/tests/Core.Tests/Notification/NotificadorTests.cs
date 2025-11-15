using Core.Notification;

namespace Core.Tests.Notification;

public class NotificadorTests
{
    [Fact]
    public void Notificador_DeveCriarSemNotificacoes()
    {
        // Arrange & Act
        var notificador = new Notificador();

        // Assert
        notificador.Should().NotBeNull();
        notificador.TemNotificacoes().Should().BeFalse();
        notificador.TemErros().Should().BeFalse();
        notificador.ObterErros().Should().BeEmpty();
        notificador.ObterInformacoes().Should().BeEmpty();
    }

    [Fact]
    public void Notificador_DeveAdicionarErro()
    {
        // Arrange
        var notificador = new Notificador();
        var mensagem = "Erro de validação";

        // Act
        notificador.AdicionarErro(mensagem);

        // Assert
        notificador.TemNotificacoes().Should().BeTrue();
        notificador.TemErros().Should().BeTrue();
        notificador.ObterErros().Should().Contain(mensagem);
        notificador.ObterInformacoes().Should().BeEmpty();
    }

    [Fact]
    public void Notificador_DeveAdicionarInformacao()
    {
        // Arrange
        var notificador = new Notificador();
        var mensagem = "Informação importante";

        // Act
        notificador.Adicionar(TipoNotificacao.Informacao, mensagem);

        // Assert
        notificador.TemNotificacoes().Should().BeTrue();
        notificador.TemErros().Should().BeFalse();
        notificador.ObterInformacoes().Should().Contain(mensagem);
        notificador.ObterErros().Should().BeEmpty();
    }

    [Fact]
    public void Notificador_DeveAdicionarMultiplasNotificacoes()
    {
        // Arrange
        var notificador = new Notificador();
        var erro1 = "Erro 1";
        var erro2 = "Erro 2";
        var info1 = "Info 1";

        // Act
        notificador.AdicionarErro(erro1);
        notificador.AdicionarErro(erro2);
        notificador.Adicionar(TipoNotificacao.Informacao, info1);

        // Assert
        notificador.TemNotificacoes().Should().BeTrue();
        notificador.TemErros().Should().BeTrue();
        notificador.ObterErros().Should().HaveCount(2);
        notificador.ObterErros().Should().Contain(erro1);
        notificador.ObterErros().Should().Contain(erro2);
        notificador.ObterInformacoes().Should().HaveCount(1);
        notificador.ObterInformacoes().Should().Contain(info1);
    }

    [Fact]
    public void Notificador_DeveAdicionarErroComMetodoGenerico()
    {
        // Arrange
        var notificador = new Notificador();
        var mensagem = "Erro genérico";

        // Act
        notificador.Adicionar(TipoNotificacao.Erro, mensagem);

        // Assert
        notificador.TemErros().Should().BeTrue();
        notificador.ObterErros().Should().Contain(mensagem);
    }

    [Fact]
    public void Notificador_DevePermitirMensagemVazia()
    {
        // Arrange
        var notificador = new Notificador();

        // Act
        notificador.AdicionarErro(string.Empty);
        notificador.Adicionar(TipoNotificacao.Informacao, string.Empty);

        // Assert
        notificador.TemNotificacoes().Should().BeTrue();
        notificador.ObterErros().Should().Contain(string.Empty);
        notificador.ObterInformacoes().Should().Contain(string.Empty);
    }

    [Fact]
    public void Notificador_DevePermitirMensagemNull()
    {
        // Arrange
        var notificador = new Notificador();

        // Act
        notificador.AdicionarErro(null!);
        notificador.Adicionar(TipoNotificacao.Informacao, null!);

        // Assert
        notificador.TemNotificacoes().Should().BeTrue();
        notificador.ObterErros().Should().Contain((string?)null);
        notificador.ObterInformacoes().Should().Contain((string?)null);
    }

    [Fact]
    public void Notificador_DeveRetornarListasVaziasQuandoSemNotificacoes()
    {
        // Arrange
        var notificador = new Notificador();

        // Act & Assert
        notificador.ObterErros().Should().BeEmpty();
        notificador.ObterInformacoes().Should().BeEmpty();
    }

    [Fact]
    public void Notificador_DeveManterNotificacoesIndependentes()
    {
        // Arrange
        var notificador1 = new Notificador();
        var notificador2 = new Notificador();

        // Act
        notificador1.AdicionarErro("Erro 1");
        notificador2.Adicionar(TipoNotificacao.Informacao, "Info 2");

        // Assert
        notificador1.TemErros().Should().BeTrue();
        notificador1.TemNotificacoes().Should().BeTrue();
        notificador2.TemErros().Should().BeFalse();
        notificador2.TemNotificacoes().Should().BeTrue();
    }

    [Fact]
    public void Estado_inicial_deve_estar_vazio_sem_erros_nem_notificacoes()
    {
        var n = new Notificador();

        n.TemNotificacoes().Should().BeFalse();
        n.TemErros().Should().BeFalse();
        n.ObterErros().Should().BeEmpty();
        n.ObterInformacoes().Should().BeEmpty();
    }

    [Fact]
    public void AdicionarErro_deve_marcar_TemErros_e_listar_em_ObterErros()
    {
        var n = new Notificador();

        n.AdicionarErro("falhou X");

        n.TemNotificacoes().Should().BeTrue();
        n.TemErros().Should().BeTrue();
        n.ObterErros().Should().ContainSingle().Which.Should().Be("falhou X");
        n.ObterInformacoes().Should().BeEmpty();
    }

    [Fact]
    public void Adicionar_informacao_deve_aparecer_em_ObterInformacoes_sem_TemErros()
    {
        var n = new Notificador();

        n.Adicionar(TipoNotificacao.Informacao, "ok 1");

        n.TemNotificacoes().Should().BeTrue();
        n.TemErros().Should().BeFalse();
        n.ObterInformacoes().Should().ContainSingle().Which.Should().Be("ok 1");
        n.ObterErros().Should().BeEmpty();
    }

    [Fact]
    public void Mistura_de_info_e_erro_deve_filtrar_corretamente()
    {
        var n = new Notificador();

        n.Adicionar(TipoNotificacao.Informacao, "i1");
        n.AdicionarErro("e1");
        n.Adicionar(TipoNotificacao.Informacao, "i2");

        n.TemNotificacoes().Should().BeTrue();
        n.TemErros().Should().BeTrue();

        n.ObterInformacoes().Should().ContainInOrder("i1", "i2");
        n.ObterErros().Should().ContainSingle().Which.Should().Be("e1");
    }

    [Fact]
    public void Ordem_de_insercao_de_erros_deve_ser_preservada_em_ObterErros()
    {
        var n = new Notificador();

        n.AdicionarErro("e1");
        n.AdicionarErro("e2");
        n.AdicionarErro("e3");

        n.ObterErros().Should().ContainInOrder("e1", "e2", "e3");
    }

    [Fact]
    public void AdicionarErro_deve_ser_equivalente_a_Adicionar_com_tipo_Erro()
    {
        var a = new Notificador();
        var b = new Notificador();

        a.AdicionarErro("boom");
        b.Adicionar(TipoNotificacao.Erro, "boom");

        a.ObterErros().Should().BeEquivalentTo(b.ObterErros(), options => options.WithStrictOrdering());
        a.TemErros().Should().Be(b.TemErros());
        a.TemNotificacoes().Should().Be(b.TemNotificacoes());
    }

    [Fact]
    public void Somente_informacoes_deve_manter_TemErros_false()
    {
        var n = new Notificador();

        n.Adicionar(TipoNotificacao.Informacao, "i1");
        n.Adicionar(TipoNotificacao.Informacao, "i2");

        n.TemErros().Should().BeFalse();
        n.ObterErros().Should().BeEmpty();
        n.ObterInformacoes().Should().HaveCount(2);
    }

    [Fact]
    public void Listas_de_retorno_devem_ser_copias_e_nao_afetar_estado_interno()
    {
        var n = new Notificador();
        n.AdicionarErro("e1");
        n.Adicionar(TipoNotificacao.Informacao, "i1");

        var erros = n.ObterErros();
        var infos = n.ObterInformacoes();

        erros.Clear();
        infos.Clear();

        n.ObterErros().Should().ContainSingle().Which.Should().Be("e1");
        n.ObterInformacoes().Should().ContainSingle().Which.Should().Be("i1");
    }

    [Fact]
    public void Mensagem_vazia_eh_aceita_e_deve_aparecer_nas_listas()
    {
        var n = new Notificador();

        n.AdicionarErro(string.Empty);
        n.Adicionar(TipoNotificacao.Informacao, string.Empty);

        n.ObterErros().Should().Contain(string.Empty);
        n.ObterInformacoes().Should().Contain(string.Empty);
    }
}
