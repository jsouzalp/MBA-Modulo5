using Core.Messages;

namespace Conteudo.Application.Commands.AtualizarCategoria;

public class AtualizarCategoriaCommand : RaizCommand
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public string IconeUrl { get; set; } = string.Empty;
    public int Ordem { get; set; } = 0;
}
