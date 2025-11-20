using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace Conteudo.Application.Commands.PublicarAula
{
    public class PublicarAulaCommandHandler(IAulaRepository aulaRepository, IMediatorHandler mediatorHandler) : IRequestHandler<PublicarAulaCommand, CommandResult>
    {
        private Guid _raizAgregacao;

        public async Task<CommandResult> Handle(PublicarAulaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _raizAgregacao = request.RaizAgregacao;

                if (!await ValidarRequisicao(request))
                    return request.Resultado;

                await aulaRepository.PublicarAulaAsync(request.CursoId, request.Id);

                if (await aulaRepository.UnitOfWork.Commit())
                {
                    request.Resultado.Data = request.Id;
                }

                return request.Resultado;
            }
            catch (Exception ex)
            {
                request.Validacao.Errors.Add(new ValidationFailure("Exception", $"Erro ao publicar aula: {ex.Message}"));
                return request.Resultado;
            }
        }

        private async Task<bool> ValidarRequisicao(PublicarAulaCommand request)
        {
            request.DefinirValidacao(new PublicarAulaCommandValidator().Validate(request));
            if (!request.EstaValido())
            {
                foreach (var erro in request.Erros)
                {
                    mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), erro)).GetAwaiter().GetResult();
                }
                return false;
            }

            var aula = await aulaRepository.ObterPorIdAsync(request.CursoId, request.Id);
            if (aula == null)
            {
                await mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), "Aula não encontrada"));
                return false;
            }

            return true;
        }
    }
}
