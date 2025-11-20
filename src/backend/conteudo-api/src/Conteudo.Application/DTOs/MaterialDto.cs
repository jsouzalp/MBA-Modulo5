using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Conteudo.Application.DTOs;

[ExcludeFromCodeCoverage]
public class CadastroMaterialDto
{
    [Required(ErrorMessage = "ID do curso é obrigatório")]
    public Guid CursoId { get; set; }

    [Required(ErrorMessage = "ID da aula é obrigatório")]
    public Guid AulaId { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo do material é obrigatório")]
    [StringLength(50, ErrorMessage = "Tipo do material deve ter no máximo 50 caracteres")]
    public string TipoMaterial { get; set; } = string.Empty;

    [Required(ErrorMessage = "URL é obrigatória")]
    [StringLength(500, ErrorMessage = "URL deve ter no máximo 500 caracteres")]
    public string Url { get; set; } = string.Empty;

    public bool IsObrigatorio { get; set; } = false;
    public long TamanhoBytes { get; set; } = 0;
    public string Extensao { get; set; } = string.Empty;
    public int Ordem { get; set; } = 0;
}

[ExcludeFromCodeCoverage]
public class AtualizarMaterialDto
{
    [Required(ErrorMessage = "ID do curso é obrigatório")]
    public Guid CursoId { get; set; }

    [Required(ErrorMessage = "ID é obrigatório")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo do material é obrigatório")]
    [StringLength(50, ErrorMessage = "Tipo do material deve ter no máximo 50 caracteres")]
    public string TipoMaterial { get; set; } = string.Empty;

    [Required(ErrorMessage = "URL é obrigatória")]
    [StringLength(500, ErrorMessage = "URL deve ter no máximo 500 caracteres")]
    public string Url { get; set; } = string.Empty;

    public bool IsObrigatorio { get; set; } = false;
    public long TamanhoBytes { get; set; } = 0;
    public string Extensao { get; set; } = string.Empty;
    public int Ordem { get; set; } = 0;
}
