using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Domain.ValueObjects;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.AtualizarCurso;

public class AtualizarCursoCommandHandler(IMediatorHandler mediatorHandler
                                        , ICursoRepository cursoRepository
                                        , ICategoriaRepository categoriaRepository)
    : IRequestHandler<AtualizarCursoCommand, CommandResult>
{
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(AtualizarCursoCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        var curso = await cursoRepository.ObterPorIdAsync(request.Id, noTracking: false);

        if (!await ValidarRequisicao(request, curso)) { return request.Resultado; }

        curso.AtualizarInformacoes(request.Nome,
                        request.Valor,
                        new ConteudoProgramatico(request.Resumo,
                                                request.Descricao,
                                                request.Objetivos,
                                                request.PreRequisitos,
                                                request.PublicoAlvo,
                                                request.Metodologia,
                                                request.Recursos,
                                                request.Avaliacao,
                                                request.Bibliografia),
                        request.DuracaoHoras,
                        request.Nivel,
                        request.Instrutor,
                        request.VagasMaximas,
                        request.ImagemUrl,
                        request.ValidoAte,
                        request.CategoriaId);

        await cursoRepository.Atualizar(curso);
        request.Resultado.Data = await cursoRepository.UnitOfWork.Commit();
        return request.Resultado;
    }

    private async Task<bool> ValidarRequisicao(AtualizarCursoCommand request, Curso curso)
    {
        request.DefinirValidacao(new AtualizarCursoCommandValidator().Validate(request));

        if (!request.EstaValido())
        {
            foreach (var erro in request.Erros)
            {
                await mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), erro));
            }
            return false;
        }

        if (curso == null)
        {
            await mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), "Curso não encontrado."));
            return false;
        }

        if (await cursoRepository.ExistePorNomeAsync(curso.Nome, curso.Id))
        {
            await mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), "Já existe um curso com este nome."));
            return false;
        }

        if (curso.CategoriaId != null && curso.CategoriaId != Guid.Empty)
        {
            var categoria = await categoriaRepository.ObterPorIdAsync((Guid)curso.CategoriaId);
            if (categoria == null)
            {
                await mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), "Categoria não encontrada"));
                return false;
            }
        }

        return true;
    }
}
