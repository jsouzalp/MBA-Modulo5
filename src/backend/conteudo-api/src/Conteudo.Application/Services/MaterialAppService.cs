using Conteudo.Application.Interfaces.Services;
using Conteudo.Domain.Interfaces.Repositories;
using Core.SharedDtos.Conteudo;
using Mapster;

namespace Conteudo.Application.Services
{
    public class MaterialAppService(IMaterialRepository materialRepository) : IMaterialAppService
    {
        public async Task<MaterialDto?> ObterPorIdAsync(Guid id)
        {
            var material = await materialRepository.ObterPorIdAsync(id);
            return material?.Adapt<MaterialDto>();
        }

        public async Task<IEnumerable<MaterialDto>> ObterPorAulaIdAsync(Guid aulaId)
        {
            var materiais = await materialRepository.ObterPorAulaIdAsync(aulaId);
            return materiais.Adapt<IEnumerable<MaterialDto>>();
        }

        public async Task<IEnumerable<MaterialDto>> ObterAtivosAsync()
        {
            var materiais = await materialRepository.ObterAtivosAsync();
            return materiais.Adapt<IEnumerable<MaterialDto>>();
        }

        public async Task<IEnumerable<MaterialDto>> ObterAtivosPorAulaIdAsync(Guid aulaId)
        {
            var materiais = await materialRepository.ObterAtivosPorAulaIdAsync(aulaId);
            return materiais.Adapt<IEnumerable<MaterialDto>>();
        }

        public async Task<IEnumerable<MaterialDto>> ObterObrigatoriosPorAulaIdAsync(Guid aulaId)
        {
            var materiais = await materialRepository.ObterObrigatoriosPorAulaIdAsync(aulaId);
            return materiais.Adapt<IEnumerable<MaterialDto>>();
        }
    }
}
