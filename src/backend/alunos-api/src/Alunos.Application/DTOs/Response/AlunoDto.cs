using System.Diagnostics.CodeAnalysis;

namespace Alunos.Application.DTOs.Response;

[ExcludeFromCodeCoverage]
public class AlunoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Telefone { get; set; }
    public bool Ativo { get; set; }
    public string Genero { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string Cep { get; set; }
    public string Foto { get; set; }

    public ICollection<MatriculaCursoDto> MatriculasCursos { get; set; }
}
