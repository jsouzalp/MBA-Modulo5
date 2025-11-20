namespace Core.SharedDtos.Conteudo;

public class AulaDto
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public string NomeCurso { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Numero { get; set; }
    public int DuracaoMinutos { get; set; }
    public string DuracaoFormatada { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string TipoAula { get; set; } = string.Empty;
    public bool IsObrigatoria { get; set; }
    public bool IsPublicada { get; set; }
    public DateTime? DataPublicacao { get; set; }
    public string Observacoes { get; set; } = string.Empty;
    public bool PodeSerVisualizada { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<MaterialDto> Materiais { get; set; } = [];
}
