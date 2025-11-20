using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.ExcluirAula
{
    public class ExcluirAulaCommandHandler(IAulaRepository aulaRepository,
                                     IMediatorHandler mediatorHandler) : IRequestHandler<ExcluirAulaCommand, CommandResult>
    {
        public async Task<CommandResult> Handle(ExcluirAulaCommand request, CancellationToken cancellationToken)
        {
            var aula = await aulaRepository.ObterPorIdAsync(request.CursoId, request.Id);

            if (!await ValidarRequisicao(request, aula))
                return request.Resultado;

            await aulaRepository.ExcluirAulaAsync(aula.CursoId, aula.Id);

            if (!await aulaRepository.UnitOfWork.Commit())
            {
                await mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Aula), "Erro ao excluir aula."));
                return request.Resultado;
            }

            request.Resultado.Data = true;
            return request.Resultado;
        }

        private async Task<bool> ValidarRequisicao(ExcluirAulaCommand request, Aula aula)
        {
            request.DefinirValidacao(new ExcluirAulaCommandValidator().Validate(request));

            if (!request.EstaValido())
            {
                foreach (var erro in request.Erros)
                {
                    await mediatorHandler.PublicarNotificacaoDominio(
                        new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Aula), erro));
                }
                return false;
            }

            if (aula == null)
            {
                await mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Aula), "Aula não encontrada."));
                return false;
            }

            return true;
        }
    }
}
