using Conteudo.Domain.Entities;
using Core.Communication;
using Core.Communication.Filters;
using Core.Data;

namespace Conteudo.Domain.Interfaces.Repositories;

public interface ICursoRepository : IRepository<Curso>
{
    Task<PagedResult<Curso>> ObterTodosAsync(CursoFilter filter);

    Task<IEnumerable<Curso>> ObterTodosAsync(bool includeAulas = false);

    Task<Curso?> ObterPorIdAsync(Guid id, bool includeAulas = false, bool noTracking = true);

    Task<IEnumerable<Curso>> ObterPorCategoriaIdAsync(Guid categoriaId, bool includeAulas = false);

    Task<bool> ExistePorNomeAsync(string nome, Guid? excludeId = null);

    Task Adicionar(Curso curso);

    Task Atualizar(Curso curso);

    Task Deletar(Curso curso);
}
