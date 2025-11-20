namespace Core.Messages.Integration
{
    public class PagamentoRecusadoEvent : IntegrationEvent
    {
        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public Guid PagamentoId { get; private set; }
        public Guid TransacaoId { get; private set; }
        public decimal Total { get; private set; }

        public PagamentoRecusadoEvent(Guid cursoId, Guid clienteId, Guid pagamentoId, Guid transacaoId, decimal total)
        {
            Id = cursoId;
            ClienteId = clienteId;
            PagamentoId = pagamentoId;
            TransacaoId = transacaoId;
            Total = total;
        }
    }
}
