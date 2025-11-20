using Core.Communication;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.ExcluirMaterial
{
    public class ExcluirMaterialCommand : RaizCommand, IRequest<CommandResult>
    {
        public Guid Id { get; }

        public ExcluirMaterialCommand(Guid id)
        {
            Id = id;
            DefinirRaizAgregacao(Id);
        }
    }
}
