using Core.Communication;
using Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace Core.Mediator;

public class MediatorHandler(IMediator mediator) : IMediatorHandler
{
    public async Task<ValidationResult> EnviarComando<T>(T comando) where T : RaizCommand
    {
        var result = await mediator.Send(comando);
        return result.ObterValidationResult();
    }

    public async Task<CommandResult> ExecutarComando<T>(T comando) where T : RaizCommand
    {
        return await mediator.Send(comando);
    }

    public async Task PublicarEvento<T>(T evento) where T : EventRaiz
    {
        await mediator.Publish(evento);
    }

    public async Task PublicarNotificacaoDominio<T>(T notificacao) where T : DomainNotificacaoRaiz
    {
        await mediator.Publish(notificacao);
    }
}
