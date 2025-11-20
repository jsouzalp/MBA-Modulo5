using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.AtualizarAula
{
    public class AtualizarAulaCommandHandler(IAulaRepository aulaRepository,
                                       ICursoRepository cursoRepository,
                                       IMediatorHandler mediatorHandler) : IRequestHandler<AtualizarAulaCommand, CommandResult>
    {
        private Guid _raizAgregacao;

        public async Task<CommandResult> Handle(AtualizarAulaCommand request, CancellationToken cancellationToken)
        {
            _raizAgregacao = request.RaizAgregacao;
            var aulaExistente = await aulaRepository.ObterPorIdAsync(request.CursoId, request.Id, false);

            if (!await ValidarRequisicao(request, aulaExistente))
                return request.Resultado;

            aulaExistente.AtualizarInformacoes(
                request.Nome,
                request.Descricao,
                request.Numero,
                request.DuracaoMinutos,
                request.VideoUrl,
                request.TipoAula,
                request.IsObrigatoria,
                request.Observacoes);

            await aulaRepository.AtualizarAulaAsync(aulaExistente);

            if (await aulaRepository.UnitOfWork.Commit())
            {
                request.Resultado.Data = true;
            }

            return request.Resultado;
        }

        private async Task<bool> ValidarRequisicao(AtualizarAulaCommand request, Aula aula)
        {
            request.DefinirValidacao(new AtualizarAulaCommandValidator().Validate(request));
            if (!request.EstaValido())
            {
                foreach (var erro in request.Erros)
                {
                    await mediatorHandler.PublicarNotificacaoDominio(
                        new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), erro));
                }
                return false;
            }

            if (aula == null)
            {
                await mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), "Aula não encontrada"));
                return false;
            }

            var curso = await cursoRepository.ObterPorIdAsync(request.CursoId);
            if (curso == null)
            {
                await mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), "Curso não encontrado"));
                return false;
            }

            if (aula.CursoId != request.CursoId)
            {
                await mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), "Aula não pertence ao curso informado"));
                return false;
            }

            if (await aulaRepository.ExistePorNumeroAsync(aula.CursoId, request.Numero, request.Id))
            {
                await mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), "Já existe uma aula com este número no curso"));
                return false;
            }

            return true;
        }
    }
}
