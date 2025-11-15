using Core.SharedDtos.Conteudo;

namespace Conteudo.Application.Interfaces.Services;

public interface IAulaAppService
{
    Task<IEnumerable<AulaDto>> ObterTodosAsync(Guid cursoId, bool includeMateriais = false);

    Task<AulaDto?> ObterPorIdAsync(Guid cursoId, Guid id, bool includeMateriais = false);

    Task<IEnumerable<AulaDto>> ObterPublicadasAsync(bool includeMateriais = false);
}
