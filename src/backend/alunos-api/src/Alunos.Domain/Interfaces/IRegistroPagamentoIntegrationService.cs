using Core.Messages;
using Core.Messages.Integration;

namespace Alunos.Domain.Interfaces;

public interface IRegistroPagamentoIntegrationService
{
    Task<ResponseMessage> ProcessarPagamentoMatriculaCursoAsync(PagamentoMatriculaCursoIntegrationEvent message);
}
