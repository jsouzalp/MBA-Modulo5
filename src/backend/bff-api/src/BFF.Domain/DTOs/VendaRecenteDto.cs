using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class VendaRecenteDto
{
    public Guid Id { get; set; }
    public string AlunoNome { get; set; } = string.Empty;
    public string CursoNome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime DataVenda { get; set; }
    public string Status { get; set; } = string.Empty;
    public string FormaPagamento { get; set; } = string.Empty;
}
