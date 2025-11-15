using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs.Pagamentos.Response;

[ExcludeFromCodeCoverage]
public class PagamentoDto
{
    public Guid Id { get; set; }
    public Guid CobrancaCursoId { get; set; }
    public Guid AlunoId { get; set; }

    public string Status { get; set; } = string.Empty;
    public decimal Valor { get; set; }

    public string NomeCartao { get; set; } = string.Empty;
    public string NumeroCartao { get; set; } = string.Empty;
    public string ExpiracaoCartao { get; set; } = string.Empty;
    public string CvvCartao { get; set; } = string.Empty;

    public TransacaoDto Transacao { get; set; } = new TransacaoDto();
}
