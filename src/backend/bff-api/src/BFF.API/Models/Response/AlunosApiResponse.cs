using System.Diagnostics.CodeAnalysis;

namespace BFF.API.Models.Response;

[ExcludeFromCodeCoverage]
public class AlunoPerfilResponse
{
    public Guid Id { get; set; }
    public Guid CodigoUsuarioAutenticacao { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public int Idade { get; set; }
    public string Telefone { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string CEP { get; set; } = string.Empty;
    public bool IsAtivo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

[ExcludeFromCodeCoverage]
public class AlunoMatriculasResponse
{
    public Guid AlunoId { get; set; }
    public string NomeAluno { get; set; } = string.Empty;
    public List<MatriculaResponse> Matriculas { get; set; } = new();
}

[ExcludeFromCodeCoverage]
public class MatriculaResponse
{
    public Guid Id { get; set; }
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public string CursoNome { get; set; } = string.Empty;
    public DateTime DataMatricula { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal PercentualConclusao { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
