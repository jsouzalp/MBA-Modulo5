namespace Core.SharedDtos.Conteudo;

public class MaterialDto
{
    public Guid Id { get; set; }
    public Guid AulaId { get; set; }
    public string NomeAula { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string TipoMaterial { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool IsObrigatorio { get; set; }
    public long TamanhoBytes { get; set; }
    public string TamanhoFormatado { get; set; } = string.Empty;
    public string Extensao { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public bool IsAtivo { get; set; }
    public bool EhArquivo { get; set; }
    public bool EhLink { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
