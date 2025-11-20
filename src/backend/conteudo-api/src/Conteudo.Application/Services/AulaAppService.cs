using Conteudo.Application.Interfaces.Services;
using Conteudo.Domain.Interfaces.Repositories;
using Core.SharedDtos.Conteudo;
using Mapster;

namespace Conteudo.Application.Services
{
    public class AulaAppService(IAulaRepository aulaRepository) : IAulaAppService
    {
        public async Task<IEnumerable<AulaDto>> ObterTodosAsync(Guid cursoId, bool includeMateriais = false)
        {
            var aulas = await aulaRepository.ObterTodosAsync(cursoId, includeMateriais);
            return aulas.Adapt<IEnumerable<AulaDto>>();
        }

        public async Task<AulaDto?> ObterPorIdAsync(Guid cursoId, Guid id, bool includeMateriais = false)
        {
            var aula = await aulaRepository.ObterPorIdAsync(cursoId, id, includeMateriais);
            return aula?.Adapt<AulaDto>();
        }

        public async Task<IEnumerable<AulaDto>> ObterPublicadasAsync(bool includeMateriais = false)
        {
            var aulas = await aulaRepository.ObterPublicadasAsync(includeMateriais);
            return aulas.Adapt<IEnumerable<AulaDto>>();
        }
    }
}
