using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.AtualizarCategoria;

public class AtualizarCategoriaCommandHandler(ICategoriaRepository categoriaRepository
                                            , IMediatorHandler mediator)
    : IRequestHandler<AtualizarCategoriaCommand, CommandResult>
{
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(AtualizarCategoriaCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.Id;
        var categoriaExistente = await categoriaRepository.ObterPorIdAsync(request.Id, false);

        if (!await ValidarRequisicao(request, categoriaExistente))
            return request.Resultado;

        categoriaExistente.AtualizarInformacoes(
            nome: request.Nome,
            descricao: request.Descricao,
            cor: request.Cor,
            iconeUrl: request.IconeUrl,
            ordem: request.Ordem
        );

        categoriaRepository.Atualizar(categoriaExistente);
        request.Resultado.Data = await categoriaRepository.UnitOfWork.Commit();

        return request.Resultado;
    }

    private async Task<bool> ValidarRequisicao(AtualizarCategoriaCommand request, Categoria categoria)
    {
        request.DefinirRaizAgregacao(request.Id);
        request.DefinirValidacao(new AtualizarCategoriaCommandValidator().Validate(request));

        if (!request.EstaValido())
        {
            foreach (var erro in request.Erros)
                await mediator.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(AtualizarCategoriaCommand), erro));
            return false;
        }

        if (categoria == null)
        {
            await mediator.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(AtualizarCategoriaCommand), "Categoria não encontrada."));
            return false;
        }

        if (await categoriaRepository.ExistePorNome(request.Nome))
        {
            await mediator.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(AtualizarCategoriaCommand), "Já existe uma categoria com este nome."));
            return false;
        }
        return true;
    }
}
