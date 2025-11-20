using Core.Messages;

namespace Conteudo.Application.Commands.AtualizarCurso;

public class AtualizarCursoCommand : RaizCommand
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public int DuracaoHoras { get; set; }
    public string Nivel { get; set; } = string.Empty;
    public string Instrutor { get; set; } = string.Empty;
    public int VagasMaximas { get; set; }
    public string ImagemUrl { get; set; } = string.Empty;
    public DateTime? ValidoAte { get; set; }
    public Guid? CategoriaId { get; set; }
    public string Resumo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Objetivos { get; set; } = string.Empty;
    public string PreRequisitos { get; set; } = string.Empty;
    public string PublicoAlvo { get; set; } = string.Empty;
    public string Metodologia { get; set; } = string.Empty;
    public string Recursos { get; set; } = string.Empty;
    public string Avaliacao { get; set; } = string.Empty;
    public string Bibliografia { get; set; } = string.Empty;

    public AtualizarCursoCommand()
    {
        DefinirRaizAgregacao(Id);
    }
}
