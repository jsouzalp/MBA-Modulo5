using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs.Alunos.Request;

[ExcludeFromCodeCoverage]
public class RegistroHistoricoAprendizadoApiRequest
{
    public Guid AlunoId { get; set; }
    public Guid MatriculaCursoId { get; set; }
    public Guid AulaId { get; set; }
    public string NomeAula { get; set; }
    public byte DuracaoMinutos { get; set; }
    public DateTime? DataTermino { get; set; }
}
