using Core.Messages;

namespace Conteudo.Application.Commands.CadastrarCategoria;

public class CadastrarCategoriaCommand : RaizCommand
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public string IconeUrl { get; set; } = string.Empty;
    public int Ordem { get; set; }

    public CadastrarCategoriaCommand()
    {
        DefinirRaizAgregacao(Guid.NewGuid());
    }
}
