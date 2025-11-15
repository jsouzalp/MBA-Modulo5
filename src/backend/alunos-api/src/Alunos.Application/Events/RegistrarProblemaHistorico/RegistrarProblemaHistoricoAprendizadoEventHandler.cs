using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Alunos.Application.Events.RegistrarProblemaHistorico;

public class RegistrarProblemaHistoricoAprendizadoEventHandler(IMediatorHandler mediatorHandler) : INotificationHandler<RegistrarProblemaHistoricoAprendizadoEvent>
{
    private Guid _raizAgregacao;

    public async Task Handle(RegistrarProblemaHistoricoAprendizadoEvent notification, CancellationToken cancellationToken)
    {
        _raizAgregacao = notification.RaizAgregacao;
        if (!ValidarRequisicao(notification)) { return; }

        await Task.CompletedTask;
    }

    private bool ValidarRequisicao(RegistrarProblemaHistoricoAprendizadoEvent notification)
    {
        notification.DefinirValidacao(new RegistrarProblemaHistoricoAprendizadoEventValidator().Validate(notification));
        if (!notification.EstaValido())
        {
            foreach (var erro in notification.Erros)
            {
                mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), erro)).GetAwaiter().GetResult();
            }
            return false;
        }

        return true;
    }
}
