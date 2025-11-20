using System.Diagnostics.CodeAnalysis;

namespace BFF.Domain.DTOs;

[ExcludeFromCodeCoverage]
public class CursoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public bool Ativo { get; set; }
    public DateTime ValidoAte { get; set; }
    public string CategoriaId { get; set; } = string.Empty;
    public string NomeCategoria { get; set; } = string.Empty;
    public int DuracaoHoras { get; set; }
    public string Nivel { get; set; } = string.Empty;
    public string ImagemUrl { get; set; } = string.Empty;
    public string Instrutor { get; set; } = string.Empty;
    public int VagasMaximas { get; set; }
    public int VagasOcupadas { get; set; }
    public int VagasDisponiveis { get; set; }
    public bool PodeSerMatriculado { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Resumo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Objetivos { get; set; } = string.Empty;
    public string PreRequisitos { get; set; } = string.Empty;
    public string PublicoAlvo { get; set; } = string.Empty;
    public string Metodologia { get; set; } = string.Empty;
    public string Recursos { get; set; } = string.Empty;
    public string Avaliacao { get; set; } = string.Empty;
    public string Bibliografia { get; set; } = string.Empty;
    public List<AulaDto> Aulas { get; set; } = new();
}
