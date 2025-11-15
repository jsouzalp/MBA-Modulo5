namespace Pagamentos.Domain.Models
{
    public class PagamentoCurso
    {
        public Guid CursoId { get; set; }
        public Guid ClienteId { get; set; }
        public decimal Total { get; set; }
        public string NomeCartao { get; set; }
        public string NumeroCartao { get; set; }
        public string ExpiracaoCartao { get; set; }
        public string CvvCartao { get; set; }
    }
}
