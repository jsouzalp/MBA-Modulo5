using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.CadastrarAula
{
    public class CadastrarAulaCommandHandler(IAulaRepository aulaRepository,
                                        ICursoRepository cursoRepository,
                                        IMediatorHandler mediatorHandler) : IRequestHandler<CadastrarAulaCommand, CommandResult>
    {
        private Guid _raizAgregacao;

        public async Task<CommandResult> Handle(CadastrarAulaCommand request, CancellationToken cancellationToken)
        {
            _raizAgregacao = request.RaizAgregacao;
            if (!await ValidarRequisicao(request))
                return request.Resultado;

            var aula = new Aula(
                request.CursoId,
                request.Nome,
                request.Descricao,
                request.Numero,
                request.DuracaoMinutos,
                request.VideoUrl,
                request.TipoAula,
                request.IsObrigatoria,
                request.Observacoes);

            await aulaRepository.CadastrarAulaAsync(aula);

            if (await aulaRepository.UnitOfWork.Commit())
                request.Resultado.Data = aula.Id;

            return request.Resultado;
        }

        private async Task<bool> ValidarRequisicao(CadastrarAulaCommand request)
        {
            request.DefinirValidacao(new CadastrarAulaCommandValidator().Validate(request));
            if (!request.EstaValido())
            {
                foreach (var erro in request.Erros)
                {
                    await mediatorHandler.PublicarNotificacaoDominio(
                        new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), erro));
                }
                return false;
            }

            var curso = await cursoRepository.ObterPorIdAsync(request.CursoId);
            if (curso == null)
            {
                await mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), "Curso não encontrado"));
                return false;
            }

            if (await aulaRepository.ExistePorNumeroAsync(request.CursoId, request.Numero))
            {
                await mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), "Já existe uma aula com este número no curso"));
                return false;
            }

            return true;
        }
    }
}
