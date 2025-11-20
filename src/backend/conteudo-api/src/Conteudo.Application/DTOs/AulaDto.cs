using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Conteudo.Application.DTOs;

[ExcludeFromCodeCoverage]
public class CadastroAulaDto
{
    [Required(ErrorMessage = "ID do curso é obrigatório")]
    public Guid CursoId { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Número da aula é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Número da aula deve ser maior que zero")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "Duração é obrigatória")]
    [Range(1, int.MaxValue, ErrorMessage = "Duração deve ser maior que zero")]
    public int DuracaoMinutos { get; set; }

    [Required(ErrorMessage = "URL do vídeo é obrigatória")]
    [StringLength(500, ErrorMessage = "URL do vídeo deve ter no máximo 500 caracteres")]
    public string VideoUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo da aula é obrigatório")]
    [StringLength(50, ErrorMessage = "Tipo da aula deve ter no máximo 50 caracteres")]
    public string TipoAula { get; set; } = string.Empty;

    public bool IsObrigatoria { get; set; } = true;
    public string Observacoes { get; set; } = string.Empty;
}

[ExcludeFromCodeCoverage]
public class AtualizarAulaDto
{
    [Required(ErrorMessage = "ID é obrigatório")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "ID do curso é obrigatório")]
    public Guid CursoId { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Número da aula é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Número da aula deve ser maior que zero")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "Duração é obrigatória")]
    [Range(1, int.MaxValue, ErrorMessage = "Duração deve ser maior que zero")]
    public int DuracaoMinutos { get; set; }

    [Required(ErrorMessage = "URL do vídeo é obrigatória")]
    [StringLength(500, ErrorMessage = "URL do vídeo deve ter no máximo 500 caracteres")]
    public string VideoUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo da aula é obrigatório")]
    [StringLength(50, ErrorMessage = "Tipo da aula deve ter no máximo 50 caracteres")]
    public string TipoAula { get; set; } = string.Empty;

    public bool IsObrigatoria { get; set; } = true;
    public string Observacoes { get; set; } = string.Empty;
}
