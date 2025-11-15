using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class EstatisticasAlunosDto
{
    public int TotalAlunos { get; set; }
    public int AlunosAtivos { get; set; }
    public int AlunosInativos { get; set; }
    public int NovasMatriculasHoje { get; set; }
    public int NovasMatriculasSemana { get; set; }
    public int NovasMatriculasMes { get; set; }
    public decimal TaxaRetencao { get; set; }
}
