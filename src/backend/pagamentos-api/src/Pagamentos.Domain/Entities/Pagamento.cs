using Core.DomainObjects;

namespace Pagamentos.Domain.Entities
{
    public class Pagamento : Entidade, IRaizAgregacao
    {
        public Guid CobrancaCursoId { get; set; }
        public Guid AlunoId { get; set; }
        public string Status { get; set; }
        public decimal Valor { get; set; }

        public string NomeCartao { get; set; }
        public string NumeroCartao { get; set; }
        public string ExpiracaoCartao { get; set; }
        public string CvvCartao { get; set; }

        public Transacao Transacao { get; set; }
    }
}
