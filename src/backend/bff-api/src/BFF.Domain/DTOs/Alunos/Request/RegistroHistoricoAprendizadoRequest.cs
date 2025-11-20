using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs.Alunos.Request;

[ExcludeFromCodeCoverage]
public class RegistroHistoricoAprendizadoRequest
{
    [Required(ErrorMessage = "ID do aluno é obrigatório")]
    public Guid AlunoId { get; set; }

    [Required(ErrorMessage = "ID da matrícula no curso é obrigatória")]
    public Guid MatriculaCursoId { get; set; }

    [Required(ErrorMessage = "ID da aula é obrigatória")]
    public Guid AulaId { get; set; }

    public DateTime? DataTermino { get; set; }
}
