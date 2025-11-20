using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Conteudo.Application.DTOs;

[ExcludeFromCodeCoverage]
public class CadastroCategoriaDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cor é obrigatória")]
    [StringLength(100, ErrorMessage = "Cor deve ter no máximo 100 caracteres")]
    public string Cor { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "URL do ícone deve ter no máximo 500 caracteres")]
    public string IconeUrl { get; set; } = string.Empty;

    public int Ordem { get; set; } = 0;
}

[ExcludeFromCodeCoverage]
public class AtualizarCategoriaDto
{
    [Required(ErrorMessage = "ID é obrigatório")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cor é obrigatória")]
    [StringLength(100, ErrorMessage = "Cor deve ter no máximo 100 caracteres")]
    public string Cor { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "URL do ícone deve ter no máximo 500 caracteres")]
    public string IconeUrl { get; set; } = string.Empty;

    public int Ordem { get; set; } = 0;
}

[ExcludeFromCodeCoverage]
public class CategoriaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public string IconeUrl { get; set; } = string.Empty;
    public bool IsAtiva { get; set; }
    public int Ordem { get; set; }
    public int TotalCursos { get; set; }
    public int CursosAtivos { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
