using Core.Messages;

namespace Core.Tests.Messages;
public class DomainNotificacaoHandlerTests
{
    [Fact]
    public async Task Handle_deve_acumular_e_obter_mensagens()
    {
        var h = new DomainNotificacaoHandler();

        await h.Handle(new DomainNotificacaoRaiz("k1", "v1"), CancellationToken.None);
        await h.Handle(new DomainNotificacaoRaiz("k2", "v2"), CancellationToken.None);

        h.TemNotificacao().Should().BeTrue();
        h.ObterMensagens().Should().BeEquivalentTo(new[] { "v1", "v2" });
        h.ObterNotificacoes().Should().HaveCount(2);
    }

    [Fact]
    public void Limpar_deve_zerar_colecao()
    {
        var h = new DomainNotificacaoHandler();
        h.ObterNotificacoes().Add(new DomainNotificacaoRaiz("k", "v")); // usando a lista interna via método público

        h.Limpar();

        h.TemNotificacao().Should().BeFalse();
        h.ObterNotificacoes().Should().BeEmpty();
    }
}
