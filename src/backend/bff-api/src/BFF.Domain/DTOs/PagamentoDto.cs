using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class PagamentoDto
{
    public Guid Id { get; set; }
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public string CursoNome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Status { get; set; } = string.Empty;
    public string FormaPagamento { get; set; } = string.Empty;
    public DateTime DataPagamento { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
