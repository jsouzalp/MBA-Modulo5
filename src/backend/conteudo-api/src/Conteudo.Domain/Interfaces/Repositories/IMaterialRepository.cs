using Conteudo.Domain.Entities;
using Core.Data;

namespace Conteudo.Domain.Interfaces.Repositories;

public interface IMaterialRepository : IRepository<Material>
{
    Task<IEnumerable<Material>> ObterTodosAsync();

    Task<Material?> ObterPorIdAsync(Guid id);

    Task<IEnumerable<Material>> ObterPorAulaIdAsync(Guid aulaId);

    Task<IEnumerable<Material>> ObterAtivosAsync();

    Task<IEnumerable<Material>> ObterAtivosPorAulaIdAsync(Guid aulaId);

    Task<IEnumerable<Material>> ObterObrigatoriosPorAulaIdAsync(Guid aulaId);

    Task<bool> ExistePorNomeAsync(Guid aulaId, string nome, Guid? excludeId = null);

    Task<Material> CadastrarMaterialAsync(Material material);

    Task<Material> AtualizarMaterialAsync(Material material);

    Task ExcluirMaterialAsync(Guid id);
}
