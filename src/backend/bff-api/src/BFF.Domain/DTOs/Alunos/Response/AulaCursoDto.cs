using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs.Alunos.Response;

[ExcludeFromCodeCoverage]
public class AulaCursoDto
{
    public Guid AulaId { get; set; }
    public Guid CursoId { get; set; }
    public string NomeAula { get; set; }
    public int OrdemAula { get; set; }
    public bool Ativo { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataTermino { get; set; }
    public bool AulaJaIniciadaRealizada { get; set; }
    public string Url { get; set; }
}
