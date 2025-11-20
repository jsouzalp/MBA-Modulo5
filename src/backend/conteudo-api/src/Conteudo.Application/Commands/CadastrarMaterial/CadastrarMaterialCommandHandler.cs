using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace Conteudo.Application.Commands.CadastrarMaterial
{
    public class CadastrarMaterialCommandHandler(IMaterialRepository materialRepository,
                                            IAulaRepository aulaRepository,
                                            IMediatorHandler mediatorHandler) : IRequestHandler<CadastrarMaterialCommand, CommandResult>
    {
        private Guid _raizAgregacao;

        public async Task<CommandResult> Handle(CadastrarMaterialCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _raizAgregacao = request.RaizAgregacao;
                if (!await ValidarRequisicao(request))
                    return request.Resultado;

                var material = new Material(
                    request.AulaId,
                    request.Nome,
                    request.Descricao,
                    request.TipoMaterial,
                    request.Url,
                    request.IsObrigatorio,
                    request.TamanhoBytes,
                    request.Extensao,
                    request.Ordem);

                await materialRepository.CadastrarMaterialAsync(material);

                if (await materialRepository.UnitOfWork.Commit())
                {
                    request.Resultado.Data = material.Id;
                }

                return request.Resultado;
            }
            catch (Exception ex)
            {
                request.Validacao.Errors.Add(new ValidationFailure("Exception", $"Erro ao cadastrar material: {ex.Message}"));
                return request.Resultado;
            }
        }

        private async Task<bool> ValidarRequisicao(CadastrarMaterialCommand request)
        {
            request.DefinirValidacao(new CadastrarMaterialCommandValidator().Validate(request));
            if (!request.EstaValido())
            {
                foreach (var erro in request.Erros)
                {
                    await mediatorHandler.PublicarNotificacaoDominio(
                        new DomainNotificacaoRaiz(_raizAgregacao, nameof(Material), erro));
                }
                return false;
            }

            var aula = await aulaRepository.ObterPorIdAsync(request.CursoId, request.AulaId);
            if (aula == null)
            {
                await mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Material), "Aula não encontrada"));
                return false;
            }

            if (await materialRepository.ExistePorNomeAsync(request.AulaId, request.Nome))
            {
                await mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Material), "Já existe um material com este nome na aula"));
                return false;
            }

            return true;
        }
    }
}
