using Core.SharedDtos.Conteudo;

namespace Conteudo.Application.Interfaces.Services;

public interface IMaterialAppService
{
    Task<MaterialDto?> ObterPorIdAsync(Guid id);

    Task<IEnumerable<MaterialDto>> ObterPorAulaIdAsync(Guid aulaId);

    Task<IEnumerable<MaterialDto>> ObterAtivosAsync();

    Task<IEnumerable<MaterialDto>> ObterAtivosPorAulaIdAsync(Guid aulaId);

    Task<IEnumerable<MaterialDto>> ObterObrigatoriosPorAulaIdAsync(Guid aulaId);
}
