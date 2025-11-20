using Alunos.Application.Commands.AtualizarPagamento;
using Alunos.Domain.Interfaces;
using Core.Mediator;
using Core.Messages;
using Core.Messages.Integration;
using Microsoft.Extensions.Logging;

namespace Alunos.Application.Integration;

public class RegistroPagamentoIntegrationService(IMediatorHandler mediatorHandler,
    ILogger<RegistroPagamentoIntegrationService> logger) : IRegistroPagamentoIntegrationService
{
    public async Task<ResponseMessage> ProcessarPagamentoMatriculaCursoAsync(PagamentoMatriculaCursoIntegrationEvent message)
    {
        try
        {
            var registrarClienteCommand = new AtualizarPagamentoMatriculaCommand(message.AlunoId, message.CursoId);

            var resultado = await mediatorHandler.EnviarComando(registrarClienteCommand);

            if (resultado.IsValid)
            {
                return new ResponseMessage(resultado);
            }
            else
            {
                logger.LogWarning("Falha na validação do comando de registro de pagamento. Id Aluno: {AlunoId} Id Curso: {CursoId}, Erros: {Erros}",
                    message.AlunoId,
                    message.CursoId,
                    string.Join(", ", resultado.Errors.Select(e => e.ErrorMessage)));
                return new ResponseMessage(resultado);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao processar evento de pagamento de matrícula registrado. Id Aluno: {AlunoId} Id Curso: {CursoId}", message.AlunoId, message.CursoId);
            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Exception", ex.Message));
            return new ResponseMessage(validationResult);
        }
    }
}
