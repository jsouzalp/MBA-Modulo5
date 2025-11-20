using Core.Communication;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.DespublicarAula
{
    public class DespublicarAulaCommand : RaizCommand, IRequest<CommandResult>
    {
        public Guid CursoId { get; private set; }
        public Guid Id { get; private set; }

        public DespublicarAulaCommand(Guid cursoId, Guid id)
        {
            CursoId = cursoId;
            Id = id;
            DefinirRaizAgregacao(Id);
        }
    }
}
