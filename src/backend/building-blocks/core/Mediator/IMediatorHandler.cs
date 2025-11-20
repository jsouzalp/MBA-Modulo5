using Core.Communication;
using Core.Messages;
using FluentValidation.Results;

namespace Core.Mediator;

public interface IMediatorHandler
{
    Task PublicarEvento<T>(T evento) where T : EventRaiz;

    Task<ValidationResult> EnviarComando<T>(T comando) where T : RaizCommand;

    Task<CommandResult> ExecutarComando<T>(T comando) where T : RaizCommand;

    Task PublicarNotificacaoDominio<T>(T notificacao) where T : DomainNotificacaoRaiz;
}
