using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.ExcluirMaterial
{
    public class ExcluirMaterialCommandHandler(IMaterialRepository materialRepository,
                                         IMediatorHandler mediatorHandler) : IRequestHandler<ExcluirMaterialCommand, CommandResult>
    {
        public async Task<CommandResult> Handle(ExcluirMaterialCommand request, CancellationToken cancellationToken)
        {
            var material = await materialRepository.ObterPorIdAsync(request.Id);

            if (!await ValidarRequisicao(request, material))
                return request.Resultado;

            await materialRepository.ExcluirMaterialAsync(material.Id);

            if (!await materialRepository.UnitOfWork.Commit())
            {
                await mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Material), "Erro ao excluir o material."));
                return request.Resultado;
            }

            request.Resultado.Data = true;
            return request.Resultado;
        }

        private async Task<bool> ValidarRequisicao(ExcluirMaterialCommand request, Material material)
        {
            request.DefinirValidacao(new ExcluirMaterialCommandValidator().Validate(request));

            if (!request.EstaValido())
            {
                foreach (var erro in request.Erros)
                {
                    await mediatorHandler.PublicarNotificacaoDominio(
                        new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Material), erro));
                }
                return false;
            }

            if (material == null)
            {
                await mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Material), "Material não encontrado."));
                return false;
            }

            return true;
        }
    }
}
