using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Alunos.Application.DTOs.Request;

[ExcludeFromCodeCoverage]
public class SolicitaCertificadoRequest
{
    [Required(ErrorMessage = "ID do aluno é obrigatório")]
    public Guid AlunoId { get; set; }

    [Required(ErrorMessage = "ID da matrícula do curso é obrigatório")]
    public Guid MatriculaCursoId { get; set; }
}
