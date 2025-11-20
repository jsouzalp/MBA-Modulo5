using System.Diagnostics.CodeAnalysis;

namespace Conteudo.Application.DTOs;

[ExcludeFromCodeCoverage]
public class ConteudoProgramaticoDto
{
    public string Resumo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Objetivos { get; set; } = string.Empty;
    public string PreRequisitos { get; set; } = string.Empty;
    public string PublicoAlvo { get; set; } = string.Empty;
    public string Metodologia { get; set; } = string.Empty;
    public string Recursos { get; set; } = string.Empty;
    public string Avaliacao { get; set; } = string.Empty;
    public string Bibliografia { get; set; } = string.Empty;
}
