using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace BFF.API.Models.Request;

[ExcludeFromCodeCoverage]
public class AulaCriarRequest
{
    [Required(ErrorMessage = "CursoId é obrigatório")]
    public Guid CursoId { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ordem é obrigatória")]
    public int Ordem { get; set; }

    [Required(ErrorMessage = "Duração é obrigatória")]
    public int DuracaoMinutos { get; set; }

    [Required(ErrorMessage = "URL do vídeo é obrigatória")]
    public string VideoUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Número da aula é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Número da aula deve ser maior que zero")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "Tipo da aula é obrigatório")]
    [StringLength(50, ErrorMessage = "Tipo da aula deve ter no máximo 50 caracteres")]
    public string TipoAula { get; set; } = string.Empty;

    public bool IsObrigatoria { get; set; } = true;
    public string Observacoes { get; set; } = string.Empty;
}
