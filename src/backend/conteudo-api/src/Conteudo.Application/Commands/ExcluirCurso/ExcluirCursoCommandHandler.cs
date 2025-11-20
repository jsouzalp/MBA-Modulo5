using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.ExcluirCurso;

public class ExcluirCursoCommandHandler(ICursoRepository cursoRepository
                                      , IMediatorHandler mediator)
    : IRequestHandler<ExcluirCursoCommand, CommandResult>
{
    public async Task<CommandResult> Handle(ExcluirCursoCommand request, CancellationToken cancellationToken)
    {
        var curso = await cursoRepository.ObterPorIdAsync(request.Id);
        if (!await ValidarRequisicao(request, curso))
            return request.Resultado;

        await cursoRepository.Deletar(curso);

        if (!await cursoRepository.UnitOfWork.Commit())
        {
            await mediator.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Curso), "Erro ao excluir o curso."));
            return request.Resultado;
        }
        request.Resultado.Data = true;
        return request.Resultado;
    }

    private async Task<bool> ValidarRequisicao(ExcluirCursoCommand request, Curso curso)
    {
        request.DefinirValidacao(new ExcluirCursoCommandValidator().Validate(request));

        if (!request.EstaValido())
        {
            foreach (var erro in request.Erros)
            {
                await mediator.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Curso), erro));
            }
            return false;
        }

        if (curso == null)
        {
            await mediator.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Curso), "Curso não encontrado."));
            return false;
        }

        if (curso.Aulas.Count > 0)
        {
            await mediator.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Curso), "Curso não pode ser excluído porque possui aulas associadas."));
            return false;
        }

        return true;
    }
}
