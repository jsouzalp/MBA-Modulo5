using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class EstatisticasCursosDto
{
    public int TotalCursos { get; set; }
    public int CursosAtivos { get; set; }
    public int CursosInativos { get; set; }
    public decimal MediaAvaliacoes { get; set; }
    public int TotalAulas { get; set; }
    public int HorasConteudo { get; set; }
}
