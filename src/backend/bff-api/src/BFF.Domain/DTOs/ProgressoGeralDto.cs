using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class ProgressoGeralDto
{
    public int CursosMatriculados { get; set; }
    public int CursosConcluidos { get; set; }
    public int CertificadosEmitidos { get; set; }
    public decimal PercentualConcluidoGeral { get; set; }
    public int HorasEstudadas { get; set; }
}
