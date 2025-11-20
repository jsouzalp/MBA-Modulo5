using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class CursoPopularDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int TotalMatriculas { get; set; }
    public decimal Receita { get; set; }
    public decimal MediaAvaliacoes { get; set; }
    public int TotalAvaliacoes { get; set; }
}
