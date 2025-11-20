using Core.Messages.Integration;

namespace Core.Tests.Messages.Integration;
public class PagamentoStatusEventsTests
{
    [Fact]
    public void PagamentoRealizadoEvent_deve_mapear_props()
    {
        var id = Guid.NewGuid();
        var cliente = Guid.NewGuid();
        var pagamento = Guid.NewGuid();
        var transacao = Guid.NewGuid();

        var evt = new PagamentoRealizadoEvent(id, cliente, pagamento, transacao, 50m);

        evt.Id.Should().Be(id);
        evt.ClienteId.Should().Be(cliente);
        evt.PagamentoId.Should().Be(pagamento);
        evt.TransacaoId.Should().Be(transacao);
        evt.Total.Should().Be(50m);
    }

    [Fact]
    public void PagamentoRecusadoEvent_deve_mapear_props()
    {
        var cursoId = Guid.NewGuid(); // na classe, parâmetro chama 'cursoId' e é guardado em 'Id'
        var cliente = Guid.NewGuid();
        var pagamento = Guid.NewGuid();
        var transacao = Guid.NewGuid();

        var evt = new PagamentoRecusadoEvent(cursoId, cliente, pagamento, transacao, 33.3m);

        evt.Id.Should().Be(cursoId);
        evt.ClienteId.Should().Be(cliente);
        evt.PagamentoId.Should().Be(pagamento);
        evt.TransacaoId.Should().Be(transacao);
        evt.Total.Should().Be(33.3m);
    }
}
