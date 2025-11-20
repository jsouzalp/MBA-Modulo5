namespace Core.Messages.Integration
{
    public class PagamentoRealizadoEvent : IntegrationEvent
    {
        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public Guid PagamentoId { get; private set; }
        public Guid TransacaoId { get; private set; }
        public decimal Total { get; private set; }

        public PagamentoRealizadoEvent(Guid Id, Guid clienteId, Guid pagamentoId, Guid transacaoId, decimal total)
        {
            this.Id = Id;
            ClienteId = clienteId;
            PagamentoId = pagamentoId;
            TransacaoId = transacaoId;
            Total = total;
        }
    }
}
