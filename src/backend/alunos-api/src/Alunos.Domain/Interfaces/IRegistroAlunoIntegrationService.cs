using Core.Messages;
using Core.Messages.Integration;

namespace Alunos.Domain.Interfaces;

/// <summary>
/// Interface para o serviço de integração de registro de usuário
/// Segue o princípio de inversão de dependência (DIP) do SOLID
/// </summary>
public interface IRegistroAlunoIntegrationService
{
    /// <summary>
    /// Processa o evento de usuário registrado e cria o perfil de aluno correspondente
    /// </summary>
    /// <param name="message">Evento de integração com dados do usuário registrado</param>
    /// <returns>Resposta indicando sucesso ou falha da operação</returns>
    Task<ResponseMessage> ProcessarAlunoRegistradoAsync(AlunoRegistradoIntegrationEvent message);
}
