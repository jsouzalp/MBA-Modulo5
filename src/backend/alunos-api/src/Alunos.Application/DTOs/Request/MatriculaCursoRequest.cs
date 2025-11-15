using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Alunos.Application.DTOs.Request;

[ExcludeFromCodeCoverage]
public class MatriculaCursoRequest
{
    [Required(ErrorMessage = "ID do aluno é obrigatório")]
    public Guid AlunoId { get; set; }

    [Required(ErrorMessage = "ID do curso é obrigatório")]
    public Guid CursoId { get; set; }

    public bool CursoDisponivel { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; }

    public decimal Valor { get; set; }

    public string Observacao { get; init; }
}
