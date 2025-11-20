using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Domain.ValueObjects;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.CadastrarCurso;

public class CadastrarCursoCommandHandler(IMediatorHandler mediatorHandler,
                                        ICursoRepository cursoRepository,
                                        ICategoriaRepository categoriaRepository) : IRequestHandler<CadastrarCursoCommand, CommandResult>
{
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(CadastrarCursoCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!await ValidarRequisicao(request)) { return request.Resultado; }

        var curso = new Curso(request.Nome,
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

        await cursoRepository.Adicionar(curso);
        if (await cursoRepository.UnitOfWork.Commit())
            request.Resultado.Data = curso.Id;
        else
            request.Resultado.Data = Guid.Empty;
        return request.Resultado;
    }

    private async Task<bool> ValidarRequisicao(CadastrarCursoCommand request)
    {
        request.DefinirValidacao(new CadastrarCursoCommandValidator().Validate(request));
        if (!request.EstaValido())
        {
            foreach (var erro in request.Erros)
            {
                mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), erro)).GetAwaiter().GetResult();
            }
            return false;
        }

        if (await cursoRepository.ExistePorNomeAsync(request.Nome))
        {
            mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), "Já existe um curso com este nome.")).GetAwaiter().GetResult();
            return false;
        }

        if (request.CategoriaId != null && request.CategoriaId != Guid.Empty)
        {
            var categoria = await categoriaRepository.ObterPorIdAsync((Guid)request.CategoriaId);
            if (categoria == null)
            {
                mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), "Categoria não encontrada")).GetAwaiter().GetResult();
                return false;
            }
        }

        return true;
    }
}
