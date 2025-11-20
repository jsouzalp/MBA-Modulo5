using Core.Communication;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.ExcluirAula
{
    public class ExcluirAulaCommand : RaizCommand, IRequest<CommandResult>
    {
        public Guid CursoId { get; set; }
        public Guid Id { get; }

        public ExcluirAulaCommand(Guid cursoId, Guid id)
        {
            CursoId = cursoId;
            Id = id;
            DefinirRaizAgregacao(Id);
        }
    }
}
