using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Conteudo.Application.DTOs;

[ExcludeFromCodeCoverage]
public class CadastroCursoDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Valor é obrigatório")]
    [Range(0, double.MaxValue, ErrorMessage = "Valor deve ser maior ou igual a zero")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "Duração é obrigatória")]
    [Range(1, int.MaxValue, ErrorMessage = "Duração deve ser maior que zero")]
    public int DuracaoHoras { get; set; }

    [Required(ErrorMessage = "Nível é obrigatório")]
    [StringLength(50, ErrorMessage = "Nível deve ter no máximo 50 caracteres")]
    public string Nivel { get; set; } = string.Empty;

    [Required(ErrorMessage = "Instrutor é obrigatório")]
    [StringLength(100, ErrorMessage = "Instrutor deve ter no máximo 100 caracteres")]
    public string Instrutor { get; set; } = string.Empty;

    [Required(ErrorMessage = "Número de vagas é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Número de vagas deve ser maior que zero")]
    public int VagasMaximas { get; set; }

    [StringLength(500, ErrorMessage = "URL da imagem deve ter no máximo 500 caracteres")]
    public string ImagemUrl { get; set; } = string.Empty;

    public DateTime? ValidoAte { get; set; }
    public Guid? CategoriaId { get; set; }

    [Required(ErrorMessage = "Resumo é obrigatório")]
    public string Resumo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Objetivos são obrigatórios")]
    public string Objetivos { get; set; } = string.Empty;

    public string PreRequisitos { get; set; } = string.Empty;
    public string PublicoAlvo { get; set; } = string.Empty;
    public string Metodologia { get; set; } = string.Empty;
    public string Recursos { get; set; } = string.Empty;
    public string Avaliacao { get; set; } = string.Empty;
    public string Bibliografia { get; set; } = string.Empty;
}

[ExcludeFromCodeCoverage]
public class AtualizarCursoDto
{
    [Required(ErrorMessage = "ID é obrigatório")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Valor é obrigatório")]
    [Range(0, double.MaxValue, ErrorMessage = "Valor deve ser maior ou igual a zero")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "Duração é obrigatória")]
    [Range(1, int.MaxValue, ErrorMessage = "Duração deve ser maior que zero")]
    public int DuracaoHoras { get; set; }

    [Required(ErrorMessage = "Nível é obrigatório")]
    [StringLength(50, ErrorMessage = "Nível deve ter no máximo 50 caracteres")]
    public string Nivel { get; set; } = string.Empty;

    [Required(ErrorMessage = "Instrutor é obrigatório")]
    [StringLength(100, ErrorMessage = "Instrutor deve ter no máximo 100 caracteres")]
    public string Instrutor { get; set; } = string.Empty;

    [Required(ErrorMessage = "Número de vagas é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Número de vagas deve ser maior que zero")]
    public int VagasMaximas { get; set; }

    [StringLength(500, ErrorMessage = "URL da imagem deve ter no máximo 500 caracteres")]
    public string ImagemUrl { get; set; } = string.Empty;

    public DateTime? ValidoAte { get; set; }
    public Guid? CategoriaId { get; set; }

    // Conteúdo Programático
    [Required(ErrorMessage = "Resumo é obrigatório")]
    public string Resumo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Objetivos são obrigatórios")]
    public string Objetivos { get; set; } = string.Empty;

    public string PreRequisitos { get; set; } = string.Empty;
    public string PublicoAlvo { get; set; } = string.Empty;
    public string Metodologia { get; set; } = string.Empty;
    public string Recursos { get; set; } = string.Empty;
    public string Avaliacao { get; set; } = string.Empty;
    public string Bibliografia { get; set; } = string.Empty;
}
