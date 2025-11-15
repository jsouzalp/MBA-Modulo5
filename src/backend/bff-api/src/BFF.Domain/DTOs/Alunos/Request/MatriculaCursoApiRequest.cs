using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs.Alunos.Request;

[ExcludeFromCodeCoverage]
public class MatriculaCursoApiRequest
{
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public bool CursoDisponivel { get; set; }
    public string Nome { get; set; }
    public decimal Valor { get; set; }
    public string Observacao { get; init; }
}
