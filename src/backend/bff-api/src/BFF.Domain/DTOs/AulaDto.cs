using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class AulaDto
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Numero { get; set; }
    public int DuracaoMinutos { get; set; }
    public string TipoAula { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
