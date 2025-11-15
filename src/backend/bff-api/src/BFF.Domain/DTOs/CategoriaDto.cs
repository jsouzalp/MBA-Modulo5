using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class CategoriaDto
{
    public string Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Cor { get; set; }
    public string IconeUrl { get; set; }
    public bool IsAtiva { get; set; }
    public int Ordem { get; set; }
    public int TotalCursos { get; set; }
    public int CursosAtivos { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
